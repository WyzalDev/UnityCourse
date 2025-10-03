// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Data;
using Core.Turrets;

namespace Core.Managers
{
    public class ProjectilesManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private Transform _projectilesParent;
        [SerializeField] private EnemyManager _enemyManager;

        private static ProjectilesManager _instance;

        private void Start()
        {
            _instance = this;
        }

        public static void InstantiateProjectile(ProjectileData projectileData, Vector3 position, Quaternion rotation)
        {
            var projectile = Instantiate(projectileData.Prefab, position, rotation, _instance._projectilesParent);
            projectile.Initialize(projectileData);
        }

        public static void DestroyProjectile(Projectile projectile, bool isEnemyHitted = false)
        {
            Destroy(projectile.gameObject);
            if (isEnemyHitted)
                _instance._enemyManager.OnEnemyHitted?.Invoke();
        }

        public void Restore()
        {
            var projectiles = _projectilesParent.GetComponentsInChildren<Projectile>();
            for (var i = 0; i < projectiles.Length; i++)
                DestroyProjectile(projectiles[i]);
        }
    }
}