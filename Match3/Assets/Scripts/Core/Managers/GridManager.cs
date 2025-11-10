// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Core.Data;
using Core.Grid;
using Core.Utils;

namespace Core.Managers
{
    public class GridManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private GoalsManager _goalsManager;
        [SerializeField] private MovesLimitationManager _movesLimitationManager;

        private FieldConfig _fieldConfig;
        private ElementsGrid _grid;
        private ElementsGridView _gridView;

        private int _rocketChainSize;
        private int _bombChainSize;

        private bool _isAnimationsSkipping;
        private Sequence _currentAnimationSequence;
        
        private const int MaxCountToMatch = 3;

        public void Initialize(FieldConfig fieldConfig, ElementsGrid grid, ElementsGridView gridView, int rocketChainSize,
            int bombChainSize)
        {
            _fieldConfig = fieldConfig;
            _grid = grid;
            _gridView = gridView;

            ViewModelCoordinatesConverter.UpdateOffsetAndSize(_gridView.transform.parent.localPosition,
                new Vector2(_gridView.SpriteWidth, _gridView.SpriteHeight));

            _rocketChainSize = rocketChainSize;
            _bombChainSize = bombChainSize;
        }

        public bool IsChainPossible(Element element)
        {
            return IsDepthFirstSearchCanReachMaxDepth(MaxCountToMatch, element);
        }

        private bool IsDepthFirstSearchCanReachMaxDepth(int maxDepth, Element element)
        {
            var visitedVertexes = new HashSet<Element>();
            var bypassStack = new Stack<Tuple<Element, int>>();

            bypassStack.Push(new Tuple<Element, int>(element, 1));

            while (bypassStack.Count > 0)
            {
                var (currentVertex, currentDepth) = bypassStack.Pop();

                if (!visitedVertexes.Add(currentVertex))
                    continue;

                if (currentDepth >= maxDepth)
                    return true;

                var startI = Mathf.Clamp(currentVertex.X - 1, 0, _grid.Height - 1);
                var endI = Mathf.Clamp(currentVertex.X + 1, 0, _grid.Height - 1);
                var startJ = Mathf.Clamp(currentVertex.Y - 1, 0, _grid.Width - 1);
                var endJ = Mathf.Clamp(currentVertex.Y + 1, 0, _grid.Width - 1);

                for (var i = startI; i <= endI; i++)
                {
                    for (var j = startJ; j <= endJ; j++)
                    {
                        var neighbor = _grid.Grid[i][j];

                        if (neighbor.Type != element.Type && neighbor.Type != ElementType.Rocket &&
                            neighbor.Type != ElementType.Bomb)
                            continue;

                        if (!visitedVertexes.Contains(neighbor))
                            bypassStack.Push(new Tuple<Element, int>(neighbor, currentDepth + 1));
                    }
                }
            }

            return false;
        }

        public void MatchElements(ICollection<Element> elements)
        {
            var bonuses = new List<Element>();
            var obstacles = new HashSet<Element>();

            foreach (var element in elements)
            {
                if (element.Type.IsGem())
                {
                    var neighbours = element.GetNeighbourObstacles();

                    obstacles.UnionWith(neighbours);

                    _goalsManager.TryAddGoalScore(element.Type);
                    element.SetElementType(ElementType.None);
                }
                else
                    bonuses.Add(element);
            }

            Action<ElementType> tryAddGoalScoreAction = _goalsManager.TryAddGoalScore;

            foreach (var bonus in bonuses)
            {
                _grid.ActivateBonusAndGetAffectedElementsCount(tryAddGoalScoreAction, bonus, out var affectedObstacles);

                foreach (var affectedObstacle in affectedObstacles)
                    obstacles.Add(affectedObstacle);
            }

            var spawningBonus = ElementType.None;

            if (elements.Count >= _bombChainSize)
                spawningBonus = ElementType.Bomb;
            else if (elements.Count >= _rocketChainSize)
                spawningBonus = ElementType.Rocket;

            var startElement = elements.FirstOrDefault();

            if (spawningBonus != ElementType.None)
            {
                startElement?.SetElementType(spawningBonus);
                _gridView.UpdateElementView(startElement);
            }

            _grid.ReduceNeighborObstaclesDurability(tryAddGoalScoreAction, obstacles);

            if (obstacles.Count != 0)
            {
                foreach (var obstacle in obstacles)
                    _gridView.UpdateElementView(obstacle);
            }

            _movesLimitationManager.OnMoveMade?.Invoke();
        }

        public ElementFallData ApplyFallingAndSpawning()
        {
            var elementsFallData = new ElementFallData();

            for (var tick = 0; _grid.HasEmptyCells(); tick++)
            {
                _grid.ApplyFallingTick(elementsFallData, tick);
                _grid.TrySpawnElements(elementsFallData, tick);
                if (!elementsFallData.FallingPath.ContainsKey(tick))
                    break;
            }

            return elementsFallData;
        }

        public Element GetElement(int x, int y)
        {
            return IsElementExistsOnCoords(x, y) ? _grid.Grid[x][y] : null;
        }

        private bool IsElementExistsOnCoords(int x, int y)
        {
            return Mathf.Clamp(x, 0, _grid.Height - 1) == x &&
                   Mathf.Clamp(y, 0, _grid.Width - 1) == y;
        }

        public void DestroyGemsAnimation()
        {
            _gridView.DestroyGemsAnimation();
        }

        public IEnumerator WaitOnDestroyGemsAnimations()
        {
            yield return _gridView.WaitOnDestroyGemsAnimation();
        }

        public IEnumerator ElementsFallingAnimation(ElementFallData elementFallData)
        {
            for (var tick = 0; elementFallData.FallingPath.ContainsKey(tick); tick++)
            {
                _currentAnimationSequence = DOTween.Sequence();
                var elementsFallPathsOnTick = elementFallData.FallingPath[tick];

                foreach (var (from, to, elementType) in elementsFallPathsOnTick)
                    _currentAnimationSequence.Join(_gridView.FallGemAnimation(from, to, elementType));

                yield return _currentAnimationSequence.WaitForCompletion();
            }
        }

        public void Restore()
        {
            for (var i = 0; i < _grid.Height; i++)
            {
                for (var j = 0; j < _grid.Width; j++)
                {
                    _grid.Grid[i][j].SetElementType(_fieldConfig.Field[i].FieldPart[j]);
                    _gridView.UpdateElementView(_grid.Grid[i][j]);
                }
            }
        }
    }
}