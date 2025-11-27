// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Audio.Managers;
using Core.Data;
using Core.Managers;
using Core.Utils;

namespace Core.Grid
{
    [Serializable]
    public class ElementsGrid
    {
        private ElementTypesGenerator _generator;

        private const int BombSize = 2;

        public Element[][] Grid { get; private set; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public void Initialize(FieldConfig fieldConfig, ElementTypesGenerator generator)
        {
            Height = fieldConfig.Height;
            Width = fieldConfig.Width;
            Grid = new Element[Height][];

            for (var i = 0; i < Height; i++)
            {
                Grid[i] = new Element[Width];

                for (var j = 0; j < Width; j++)
                    Grid[i][j] = new Element(this, fieldConfig.Field[i].FieldPart[j], i, j);
            }

            _generator = generator;
        }

        public void ActivateBonusAndGetAffectedObstacles(Action<ElementType> tryAddGoalScoreAction, Element bonus,
            out HashSet<Element> affectedObstacles)
        {
            affectedObstacles = new HashSet<Element>();

            switch (bonus.Type)
            {
                case ElementType.Rocket:
                    for (var j = 0; j < Width; j++)
                    {
                        if (Grid[bonus.X][j].Type == ElementType.None)
                            continue;

                        if (Grid[bonus.X][j].Type.IsBonus() && Grid[bonus.X][j] != bonus)
                            continue;

                        if (Grid[bonus.X][j].Type.IsObstacle())
                        {
                            affectedObstacles.Add(Grid[bonus.X][j]);
                            continue;
                        }

                        tryAddGoalScoreAction?.Invoke(Grid[bonus.X][j].Type);
                        Grid[bonus.X][j].SetElementType(ElementType.None);
                    }

                    AudioManager.PlaySfxWithPitch("RocketActivation");
                    EffectManager.SpawnRocketEffect(bonus.X);

                    break;

                case ElementType.Bomb:
                    var startI = Mathf.Clamp(bonus.X - BombSize, 0, Height - 1);
                    var endI = Mathf.Clamp(bonus.X + BombSize, 0, Height - 1);
                    var startJ = Mathf.Clamp(bonus.Y - BombSize, 0, Width - 1);
                    var endJ = Mathf.Clamp(bonus.Y + BombSize, 0, Width - 1);

                    for (var i = startI; i <= endI; i++)
                    {
                        for (var j = startJ; j <= endJ; j++)
                        {
                            if (Grid[i][j].Type == ElementType.None)
                                continue;

                            if (Grid[i][j].Type.IsBonus() && Grid[i][j] != bonus)
                                continue;

                            if (Grid[i][j].Type.IsObstacle())
                            {
                                affectedObstacles.Add(Grid[i][j]);
                                continue;
                            }

                            tryAddGoalScoreAction?.Invoke(Grid[i][j].Type);
                            Grid[i][j].SetElementType(ElementType.None);
                        }
                    }

                    AudioManager.PlaySfxWithPitch("BombActivation");
                    EffectManager.SpawnBombEffect(bonus.X, bonus.Y, BombSize);

                    break;
            }
        }

        public void ReduceNeighborObstaclesDurability(Action<ElementType> tryAddGoalScoreAction,
            ICollection<Element> obstacles)
        {
            if (obstacles.Count == 0)
                return;

            if (obstacles.FirstOrDefault(x => x.Type is ElementType.Ice or ElementType.BrokenIce) != null)
                AudioManager.PlaySfxWithPitch("IceCrush");

            if (obstacles.FirstOrDefault(x => x.Type is ElementType.Rock or ElementType.BrokenRock) != null)
                AudioManager.PlaySfxWithPitch("RockCrush");

            foreach (var obstacle in obstacles)
            {
                tryAddGoalScoreAction?.Invoke(obstacle.Type);

                switch (obstacle.Type)
                {
                    case ElementType.Ice:
                        obstacle.SetElementType(ElementType.BrokenIce);
                        break;

                    case ElementType.Rock:
                        obstacle.SetElementType(ElementType.BrokenRock);
                        break;

                    case ElementType.BrokenIce:
                    case ElementType.BrokenRock:
                        obstacle.SetElementType(ElementType.None);
                        break;
                }
            }
        }

        public bool HasEmptyCells()
        {
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    if (Grid[i][j].Type == ElementType.None)
                        return true;
                }
            }

            return false;
        }

        public void TrySpawnElements(ElementFallData elementsFallData, int tick)
        {
            for (var j = 0; j < Width; j++)
            {
                if (Grid[0][j].Type != ElementType.None)
                    continue;

                var nextElementType = _generator.GetNextGemType();
                Grid[0][j].SetElementType(nextElementType);
                elementsFallData.AddFallingPoint(new Vector2Int(-1, j), new Vector2Int(0, j), nextElementType, tick);
            }
        }

        public void ApplyFallingTick(ElementFallData elementFallData, int tick)
        {
            for (var i = Height - 2; i >= 0; i--)
            {
                for (var j = 0; j < Width; j++)
                {
                    if (!Grid[i][j].CanFall())
                        continue;

                    var fallingElementVector = new Vector2Int(i, j);
                    var nextFallingPoint = Grid[i][j].NextFallingPoint();

                    Grid[nextFallingPoint.x][nextFallingPoint.y].SetElementType(Grid[i][j].Type);
                    Grid[i][j].SetElementType(ElementType.None);

                    elementFallData.AddFallingPoint(fallingElementVector, nextFallingPoint,
                        Grid[nextFallingPoint.x][nextFallingPoint.y].Type, tick);
                }
            }
        }
    }
}