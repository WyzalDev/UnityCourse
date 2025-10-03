// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Movements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : Movement
    {
        private Camera _camera;
        private InputAction _move;
        private Vector2 _targetPosition;

        protected override void Start()
        {
            base.Start();

            _camera = Camera.main;
            _move = InputSystem.actions.FindAction("Move");
            _move.performed += OnPositionChanged;
        }

        private void OnPositionChanged(InputAction.CallbackContext context)
        {
            var screenPosition = context.ReadValue<Vector2>();
            var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            _targetPosition = new Vector2(worldPosition.x, transform.position.y);
        }

        protected override void FixedUpdate()
        {
            if (IsPaused)
                return;

            _rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, _targetPosition,
                _shipSpeed * Time.fixedDeltaTime));
        }

        protected override void OnDestroy()
        {
            _move.performed -= OnPositionChanged;
        }
    }
}