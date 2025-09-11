// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.InputSystem;
using Core.Data;

namespace Core.MovingContainer
{
    public class PlayerMovement : MonoBehaviour, IPausable, IBoostable
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _maxMoveDistance;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animationSpeedMultiplier;

        [Header("Boost Effects Settings")]
        [SerializeField] private ParticleSystem _firstEngineFireEffect;
        [SerializeField] private ParticleSystem _secondEngineFireEffect;

        public bool IsPaused { get; set; }

        private InputAction _move;
        private float _currentTilt = 0.5f;
        private float _defaultEngineFireEffectLifetime;

        private void Start()
        {
            _move = InputSystem.actions.FindAction("Move");
        }

        public void OnBoostStart(BoostData startParameters)
        {
            var firstMain = _firstEngineFireEffect.main;
            var secondMain = _secondEngineFireEffect.main;

            firstMain.startLifetime = startParameters.BoostEffectLifetime;
            secondMain.startLifetime = startParameters.BoostEffectLifetime;
        }

        public void OnBoostEnd(BoostData endParameters)
        {
            var firstMain = _firstEngineFireEffect.main;
            var secondMain = _secondEngineFireEffect.main;

            firstMain.startLifetime = endParameters.BoostEffectLifetime;
            secondMain.startLifetime = endParameters.BoostEffectLifetime;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            var direction = _move.ReadValue<float>();
            var newXPosition = Mathf.Clamp(transform.position.x + 1 * direction * _speed * Time.deltaTime,
                -_maxMoveDistance, _maxMoveDistance);

            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

            var targetTilt = direction switch
            {
                -1 => 0f,
                1 => 1f,
                _ => 0.5f
            };

            _currentTilt = Mathf.MoveTowards(_currentTilt, targetTilt, Time.deltaTime * _animationSpeedMultiplier);
            _animator.SetFloat("Tilt", _currentTilt);
        }
    }
}