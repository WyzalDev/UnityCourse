// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using DG.Tweening;

namespace Core.Grid
{
    public class ElementView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
        [SerializeField] private SpriteRenderer _elementSpriteRenderer;

        [Header("Animation Settings")]
        [SerializeField] private Vector2 _selectedElementSize;
        [SerializeField] private Vector2 _defaultElementSize;
        [SerializeField] private Color _selectedElementColor;
        [SerializeField] private Color _defaultElementColor;

        public Element Element { get; private set; }

        public float Width => _elementSpriteRenderer.sprite.bounds.size.x;

        public float Height => _elementSpriteRenderer.sprite.bounds.size.y;

        public void Initialize(Element element, Sprite sprite, Sprite backgroundSprite)
        {
            Element = element;
            _elementSpriteRenderer.sprite = sprite;
            _backgroundSpriteRenderer.sprite = backgroundSprite;
        }

        public void UpdateView(Sprite sprite)
        {
            _elementSpriteRenderer.sprite = sprite;
        }

        public void PlaySelect()
        {
            transform.localScale = _selectedElementSize;
            _elementSpriteRenderer.color = _selectedElementColor;
        }

        public void PlayDeselect()
        {
            transform.localScale = _defaultElementSize;
            _elementSpriteRenderer.color = _defaultElementColor;
        }

        public Tween DestroyAnimation(float duration)
        {
            return _elementSpriteRenderer.transform.DOScale(Vector3.zero, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _elementSpriteRenderer.sprite = null;
                    _elementSpriteRenderer.transform.localScale = Vector3.one;
                });
        }
    }
}