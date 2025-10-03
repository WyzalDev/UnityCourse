// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Data;

namespace Core.Movements
{
    public class EnemyMovement : Movement
    {
        public Action OnPathEnded;

        private bool _isPathEnded;
        private List<DestinationPoint> _movePoints;
        private int _currentPointIndex;

        public void SetPath(List<DestinationPoint> path)
        {
            _movePoints = path;
        }

        protected override void FixedUpdate()
        {
            if (IsPaused)
                return;

            if (_isPathEnded)
                return;

            _rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, _movePoints[_currentPointIndex].Position,
                _shipSpeed * Time.fixedDeltaTime));

            if ((Vector2.Distance(_movePoints[_currentPointIndex].Position, _rigidbody2D.position) < 0.3f))
                OnEnemyMovementPointReached();
        }

        private void OnEnemyMovementPointReached()
        {
            if (_currentPointIndex != -1 && _currentPointIndex + 1 < _movePoints.Count)
                _currentPointIndex++;
            else
            {
                _isPathEnded = true;
                OnPathEnded?.Invoke();
            }
        }
    }
}