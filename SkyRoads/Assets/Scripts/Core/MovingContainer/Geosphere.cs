// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Data;

namespace Core.MovingContainer
{
    public class Geosphere : MonoBehaviour, IBoostable, IPausable, IRestorable
    {
        [SerializeField] private float _scalingSpeed;

        [Header("Boost Settings")]
        [SerializeField] private float _boostTransitionDuration;

        public bool IsPaused { get; set; }
        private float MaxScalingOffset => _scalingSpeed * Time.fixedDeltaTime * _currentBoostMultiplier;

        private float _currentBoostMultiplier;
        private Vector3 _startScale;
        private float _currentScalingOffset;
        private float _lerpControl;

        private void Start()
        {
            _startScale = transform.localScale;
            _currentBoostMultiplier = 1f;
            _currentScalingOffset = MaxScalingOffset;
        }

        public void Restore()
        {
            transform.localScale = _startScale;
        }

        public void OnBoostStart(BoostData startParameters)
        {
            _currentBoostMultiplier = startParameters.SpeedMultiplier;
            _lerpControl = 0f;
        }

        public void OnBoostEnd(BoostData endParameters)
        {
            _currentBoostMultiplier = endParameters.SpeedMultiplier;
            _lerpControl = 0f;
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            if (!Mathf.Approximately(MaxScalingOffset, _currentScalingOffset))
            {
                _lerpControl = Mathf.Clamp(_lerpControl + Time.fixedDeltaTime / _boostTransitionDuration,
                    0f, _boostTransitionDuration);
                
                _currentScalingOffset = Mathf.Lerp(_currentScalingOffset, MaxScalingOffset, _lerpControl);
            }

            transform.localScale += Vector3.one * _currentScalingOffset;
        }
    }
}