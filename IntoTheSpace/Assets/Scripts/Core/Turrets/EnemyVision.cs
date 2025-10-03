// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.Movements;

namespace Core.Turrets
{
    public class EnemyVision : MonoBehaviour
    {
        public Action OnPlayerInSight;
        public Action OnPlayerOutSight;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerMovement>(out var player))
                OnPlayerInSight?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerMovement>(out var player))
                OnPlayerOutSight?.Invoke();
        }
    }
}