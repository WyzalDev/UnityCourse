// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.MovingContainer
{
    public class PlayerMovement : MonoBehaviour, IPausable
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _maxMoveDistance;

        public bool IsPaused { get; set; }

        private InputAction _move;

        private void Start()
        {
            _move = InputSystem.actions.FindAction("Move");
        }

        private void Update()
        {
            if (IsPaused)
                return;

            var direction = _move.ReadValue<float>();
            var newXPosition = Mathf.Clamp(transform.position.x + 1 * direction * _speed * Time.deltaTime,
                -_maxMoveDistance, _maxMoveDistance);

            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        }
    }
}