// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
using DG.Tweening;
using Core.Data;
using Core.Utils;

namespace Core.Grid
{
    public class ElementsGridView : MonoBehaviour
    {
        private ElementsGrid _grid;
        private ElementView _elementViewPrefab;

        private Dictionary<ElementType, Sprite> _sprites = new();
        private Dictionary<BackgroundElementType, List<Sprite>> _backgroundSprites = new();
        private Dictionary<Vector2Int, ElementView> _viewsDictionary = new();

        private SpriteRenderer _animationSpritePrefab;
        private ObjectPool<SpriteRenderer> _animatedSpritesPool;
        private float _destroyGemsAnimationDuration;
        private float _moveGemsAnimationDuration;
        private Sequence _destroyGemsSequence;

        public float SpriteWidth { get; private set; }

        public float SpriteHeight { get; private set; }

        public void Initialize(ElementsGrid grid, ElementsConfig elementsConfig, Transform gemAnimationsParent)
        {
            _destroyGemsAnimationDuration = elementsConfig.DestroyGemsAnimationDuration;
            _moveGemsAnimationDuration = elementsConfig.MoveGemsAnimationDuration;
            _animationSpritePrefab = elementsConfig.AnimationSpritePrefab;
            _animatedSpritesPool = new ObjectPool<SpriteRenderer>(
                () => Instantiate(_animationSpritePrefab, gemAnimationsParent),
                x => x.gameObject.SetActive(true),
                x => x.gameObject.SetActive(false),
                x => Destroy(x.gameObject),
                true,
                grid.Height + grid.Width,
                grid.Width * grid.Height
            );

            _grid = grid;
            _elementViewPrefab = elementsConfig.ElementViewPrefab;

            SpriteWidth = _elementViewPrefab.Width;
            SpriteHeight = _elementViewPrefab.Height;

            FillSpritesDictionary(elementsConfig);
            FillBackgroundSpritesDictionary(elementsConfig);

            InstantiateBoardWithGems();
            transform.parent.position = -GetCenterPoint();
            InstantiateBoardBorders();
        }

        private void FillSpritesDictionary(ElementsConfig elementsConfig)
        {
            foreach (var elementConfig in elementsConfig.ElementConfigs)
            {
                if (!_sprites.ContainsKey(elementConfig.ElementType))
                {
                    _sprites.Add(elementConfig.ElementType, elementConfig.Sprite);
                    continue;
                }

#if UNITY_EDITOR
                Debug.LogWarning("ElementSpritesConfig contains repeated sprites for one element type");
#endif
            }

#if UNITY_EDITOR
            foreach (var enumName in Enum.GetNames(typeof(ElementType)))
            {
                var elementType = Enum.Parse<ElementType>(enumName);
                if (elementType != ElementType.None && !_sprites.ContainsKey(elementType))
                    Debug.LogWarning(
                        $"Config don't contains sprite for {elementType} ElementType, can behave unpredictable");
            }
#endif
        }

        private void FillBackgroundSpritesDictionary(ElementsConfig elementsConfig)
        {
            foreach (var backgroundElementConfig in elementsConfig.BackgroundElementConfigs)
            {
                if (!_backgroundSprites.ContainsKey(backgroundElementConfig.BackgroundElementType))
                {
                    _backgroundSprites.Add(backgroundElementConfig.BackgroundElementType,
                        backgroundElementConfig.PossibleSprites);
                    continue;
                }

#if UNITY_EDITOR
                Debug.LogWarning("BackgroundElementSpritesConfig contains repeated sprite lists for one element type");
#endif
            }

#if UNITY_EDITOR
            foreach (var enumName in Enum.GetNames(typeof(BackgroundElementType)))
            {
                var elementType = Enum.Parse<BackgroundElementType>(enumName);
                if (elementType != BackgroundElementType.None && !_backgroundSprites.ContainsKey(elementType))
                    Debug.LogWarning(
                        $"Config don't contains sprite list for {elementType} BackgroundElementType, can behave unpredictable");
            }
#endif
        }

        private void InstantiateBoardWithGems()
        {
            var nextBackgroundSpriteIndex = 0;
            var middleBackgroundSpritesSize = _backgroundSprites[BackgroundElementType.Middle].Count;

            for (var i = 0; i < _grid.Height; i++)
            {
                for (var j = 0; j < _grid.Width; j++)
                {
                    InstantiateGem(_grid.Grid[i][j], nextBackgroundSpriteIndex);
                    nextBackgroundSpriteIndex++;

                    if (nextBackgroundSpriteIndex == middleBackgroundSpritesSize)
                        nextBackgroundSpriteIndex = 0;
                }
            }
        }

        private void InstantiateGem(Element element, int nextBackgroundSpriteIndex)
        {
            var fieldCell = Instantiate(_elementViewPrefab,
                new Vector3(element.Y * SpriteWidth, -element.X * SpriteHeight, 0),
                Quaternion.identity, transform);

            _viewsDictionary.Add(new Vector2Int(element.X, element.Y), fieldCell);

            fieldCell.Initialize(element, _sprites[element.Type],
                _backgroundSprites[BackgroundElementType.Middle][nextBackgroundSpriteIndex]);
        }

        private Vector3 GetCenterPoint()
        {
            var renderers = transform.parent.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
                return transform.position;

            var totalBounds = renderers[0].bounds;

            for (var i = 1; i < renderers.Length; i++)
                totalBounds.Encapsulate(renderers[i].bounds);

            return totalBounds.center;
        }

        private void InstantiateBoardBorders()
        {
#if UNITY_EDITOR
            if (!_backgroundSprites.ContainsKey(BackgroundElementType.GemMask))
                Debug.LogWarning($"There is no sprite for {BackgroundElementType.GemMask} BackgroundElementType");
#endif

            InstantiateBoardBorder(-1, -1, BackgroundElementType.TopLeftCorner);
            InstantiateBoardBorder(-1, _grid.Height, BackgroundElementType.BottomLeftCorner);
            InstantiateBoardBorder(_grid.Width, -1, BackgroundElementType.TopRightCorner);
            InstantiateBoardBorder(_grid.Width, _grid.Height, BackgroundElementType.BottomRightCorner);

            for (var i = 0; i < _grid.Width; i++)
            {
                InstantiateBoardBorder(i, -1, BackgroundElementType.Top);
                InstantiateBoardBorder(i, _grid.Height, BackgroundElementType.Bottom);
            }

            for (var i = 0; i < _grid.Height; i++)
            {
                InstantiateBoardBorder(-1, i, BackgroundElementType.MiddleLeft);
                InstantiateBoardBorder(_grid.Width, i, BackgroundElementType.MiddleRight);
            }
        }

        private void InstantiateBoardBorder(int x, int y, BackgroundElementType type)
        {
            var border = new GameObject(type.ToString());
            var spriteRenderer = border.AddComponent<SpriteRenderer>();
            var mask = border.AddComponent<SpriteMask>();

            mask.sprite = _backgroundSprites[BackgroundElementType.GemMask].FirstOrDefault();
            border.transform.SetParent(transform);
            border.transform.localPosition = new Vector3(x * SpriteWidth, -y * SpriteHeight, 0);

            if (_backgroundSprites.TryGetValue(type, out var backgroundSprites) && backgroundSprites.Count > 0)
            {
                spriteRenderer.sprite = backgroundSprites[Random.Range(0, backgroundSprites.Count - 1)];
                return;
            }

#if UNITY_EDITOR
            Debug.LogWarning($"There is no sprite for {type.ToString()} BackgroundElementType");
#endif
        }

        public Sprite GetSpriteByType(ElementType type)
        {
            return _sprites.GetValueOrDefault(type);
        }

        public void UpdateElementView(Element element)
        {
            var elementView = _viewsDictionary[new Vector2Int(element.X, element.Y)];

            if (_sprites.TryGetValue(element.Type, out var sprite))
                elementView.UpdateView(sprite);
        }

        public ElementView GetElementViewByPosition(int x, int y)
        {
            return _viewsDictionary[new Vector2Int(x, y)];
        }

        public void DestroyGemsAnimation()
        {
            _destroyGemsSequence = DOTween.Sequence();

            foreach (var (_, view) in _viewsDictionary)
            {
                if (view.Element.Type == ElementType.None)
                    _destroyGemsSequence.Join(view.DestroyAnimation(_destroyGemsAnimationDuration));
            }
        }

        public IEnumerator WaitOnDestroyGemsAnimation()
        {
            if (_destroyGemsSequence != null && !_destroyGemsSequence.IsComplete())
                yield return _destroyGemsSequence.WaitForCompletion();
        }

        public Tween FallGemAnimation(Vector2Int from, Vector2Int to, ElementType movedType)
        {
            var toView = _viewsDictionary[to];
            var animationSprite = _animatedSpritesPool.Get();
            var toViewCords = ViewModelCoordinatesConverter.GetViewCoordinates(to);
            animationSprite.transform.position = ViewModelCoordinatesConverter.GetViewCoordinates(from);

            if (_viewsDictionary.ContainsKey(from))
                _viewsDictionary[from].UpdateView(null);

            animationSprite.sprite = _sprites.GetValueOrDefault(movedType);

            return animationSprite.transform.DOMove(toViewCords, _moveGemsAnimationDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    toView.UpdateView(animationSprite.sprite);
                    _animatedSpritesPool.Release(animationSprite);
                });
        }
    }
}