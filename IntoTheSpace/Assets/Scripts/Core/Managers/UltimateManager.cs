// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Managers
{
    public class UltimateManager : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;

        private Ultimate _currentUltimate;

        public void SetNewPlayerUltimate(Ultimate ultimate)
        {
            ultimate.OnUltimateUsed += _enemyManager.AllEnemiesTakeDamage;
            _currentUltimate = ultimate;
        }

        public void OnPlayerDestroyed()
        {
            _currentUltimate.OnUltimateUsed -= _enemyManager.AllEnemiesTakeDamage;
            _currentUltimate = null;
        }
    }
}