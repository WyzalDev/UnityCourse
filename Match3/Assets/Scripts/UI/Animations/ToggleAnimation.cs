// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Animations
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleAnimation : MonoBehaviour
    {
        [SerializeField] private Image _toggleSprite;
        [SerializeField] private float _startPivot;
        [SerializeField] private float _endPivot;
        [SerializeField] private float _toggleDuration;

        private Toggle _toggle;
        private Tween _tween;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.onValueChanged.AddListener(OnValueChanged);

            if (!_toggle.isOn)
                _toggleSprite.rectTransform.pivot = new Vector2(_endPivot, _toggleSprite.rectTransform.pivot.y);
        }

        private void OnValueChanged(bool value)
        {
            if (_tween != null && _tween.IsPlaying())
                _tween.Kill(true);

            _toggleSprite.rectTransform.DOPivotX(value ? _startPivot : _endPivot, _toggleDuration).SetUpdate(true);
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}