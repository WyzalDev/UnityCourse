// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Managers;
using Core.Data;

namespace Core.MovingContainer
{
    public class ForwardMover : MonoBehaviour, IBoostable, IPausable, IRestorable
    {
        [Header("Speed Settings")]
        [SerializeField] private float _startSpeed;
        [Tooltip("Multiplier Growth Rate dependent on time")]
        [SerializeField] private float _speedGrowthRate;

        [Header("Boost Settings")]
        [SerializeField] private float _boostTransitionDuration;

        public bool IsPaused { get; set; }

        private float MaxSpeed => (_startSpeed + _speedGrowthRate * Timer.GameDifficultMultiplier)
                                  * Time.fixedDeltaTime * _currentBoostMultiplier;

        private Rigidbody _rigidbody;
        private float _currentBoostMultiplier = 1f;
        private float _currentSpeed;
        private float _lerpControl;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentSpeed = MaxSpeed;
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

        public void Restore()
        {
            transform.position = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            if (!Mathf.Approximately(MaxSpeed, _currentSpeed))
            {
                _lerpControl = Mathf.Clamp(_lerpControl + Time.fixedDeltaTime / _boostTransitionDuration,
                    0f, _boostTransitionDuration);
                
                _currentSpeed = Mathf.Lerp(_currentSpeed, MaxSpeed, _lerpControl);
            }

            var offset = new Vector3(0, 0, _currentSpeed);
            _rigidbody.MovePosition(transform.position + offset);
        }
    }
}