// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Core.Managers
{
    public class MovesLimitationManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private TMP_Text _movesLimitationText;

        [Header("Animation Settings")]
        [SerializeField] private int _criticalLowMovesAmount;
        [SerializeField] private Color _criticalLowMovesColor;
        [SerializeField] private float _criticalLowAnimationDuration;

        public Action OnMoveMade;

        private int _requiredMovesLimitation;
        private int _currentMovesLimitation;

        private Tween _flashingTween;

        public bool IsMovesLimitationAchieved { get; private set; }

        public void Initialize(int movesLimitation)
        {
            _requiredMovesLimitation = movesLimitation;
            _currentMovesLimitation = 0;

            _movesLimitationText.text = $"{_requiredMovesLimitation - _currentMovesLimitation}";
            IsMovesLimitationAchieved = false;
        }

        private void Start()
        {
            OnMoveMade += MoveMade;
            TrySetLoopAnimation();
        }

        private void MoveMade()
        {
            if (IsMovesLimitationAchieved)
                return;

            _currentMovesLimitation++;
            _movesLimitationText.text = $"{_requiredMovesLimitation - _currentMovesLimitation}";
            ;
            IsMovesLimitationAchieved = _currentMovesLimitation >= _requiredMovesLimitation;

            TrySetLoopAnimation();
        }

        private void TrySetLoopAnimation()
        {
            if (_criticalLowMovesAmount < _requiredMovesLimitation - _currentMovesLimitation ||
                _flashingTween != null)
                return;

            var sequence = DOTween.Sequence();

            sequence.Append(_movesLimitationText.DOColor(_criticalLowMovesColor, _criticalLowAnimationDuration));
            sequence.Append(_movesLimitationText.DOColor(Color.white, _criticalLowAnimationDuration));
            sequence.SetEase(Ease.InOutExpo);

            _flashingTween = sequence;
            _flashingTween.SetAutoKill(false);
            _flashingTween.OnComplete(() => _flashingTween.Restart());
        }

        private void SmoothKillFlashingTween()
        {
            if (_flashingTween == null)
                return;

            _flashingTween.OnComplete(null);
            _flashingTween.SetAutoKill(true);
        }

        private void OnDestroy()
        {
            OnMoveMade -= MoveMade;

            _flashingTween?.Kill();
        }

        public void Restore()
        {
            _currentMovesLimitation = 0;
            _movesLimitationText.text = $"{_requiredMovesLimitation - _currentMovesLimitation}";
            SmoothKillFlashingTween();
            IsMovesLimitationAchieved = false;
        }
    }
}