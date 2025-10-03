// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Data;
using Core.Data.TurretTypes;

namespace Core.Turrets
{
    public abstract class Turret : MonoBehaviour, IPausable, IRestorable, IInitializable
    {
        [Header("Turret Type Settings")]
        [SerializeField] protected Transform _turretFirePoint;

        protected ProjectileData _projectileData;
        protected BaseTurretConfig BaseTurretConfig;

        private float _fireRate;
        private float _difficultMultiplier;
        private float _currentFireCooldown;
        private bool _isFiring;

        public bool IsPaused { get; set; }

        protected abstract void Start();

        public void Initialize(object data)
        {
            if (data is not TurretData turretData)
                return;

            _difficultMultiplier = turretData.DifficultMultiplier;
            BaseTurretConfig = turretData.turretConfig;
            OnConfigUpdated();
        }

        protected virtual void OnConfigUpdated()
        {
            _fireRate = BaseTurretConfig.FireRate;
            _projectileData = new ProjectileData()
            {
                Prefab = BaseTurretConfig.ProjectileData.Prefab,
                Damage = BaseTurretConfig.ProjectileData.Damage,
                LifeTime = BaseTurretConfig.ProjectileData.LifeTime / _difficultMultiplier,
                Speed = BaseTurretConfig.ProjectileData.Speed * _difficultMultiplier,
            };
        }

        protected void StartFire()
        {
            _isFiring = true;
        }

        protected void StopFire()
        {
            _isFiring = false;
        }

        protected virtual void Update()
        {
            if (IsPaused)
                return;

            if (_currentFireCooldown != 0)
                _currentFireCooldown = Mathf.Clamp(_currentFireCooldown - Time.deltaTime, 0, _fireRate);
            else if (_isFiring)
            {
                _currentFireCooldown = _fireRate;
                Fire();
            }
        }

        protected abstract void Fire();

        public void Restore()
        {
            _isFiring = false;
            _currentFireCooldown = 0;
        }

        protected abstract void OnDestroy();
    }
}