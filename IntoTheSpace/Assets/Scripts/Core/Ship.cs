// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.Data;
using Core.Movements;
using Core.Turrets;

namespace Core
{
    [RequireComponent(typeof(Movement), typeof(Turret))]
    public class Ship : MonoBehaviour, IRestorable, IDamageable
    {
        [SerializeField] private ShipConfig shipConfig;

        public Action<Ship, int> OnShipDestroyed;

        private IInitializable _movement;
        private IInitializable _turret;
        private IInitializable _ultimate;
        private int _health;
        private int _currentHealth;
        private float _waveModifier;

        public IInitializable Movement => _movement;
        public IInitializable Turret => _turret;
        public IInitializable Ultimate => _ultimate;

        private int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                if (value == 0)
                    OnShipDestroyed?.Invoke(this, _health);
            }
        }

        private void Awake()
        {
            _movement = GetComponent<Movement>();
            _turret = GetComponent<Turret>();
            if (TryGetComponent<Ultimate>(out var ultimate))
                _ultimate = ultimate;
        }

        private void Start()
        {
            _health = Mathf.RoundToInt(shipConfig.Health * _waveModifier);
            Restore();

            _movement.Initialize(shipConfig.Speed);

            _turret.Initialize(new TurretData()
            {
                turretConfig = shipConfig.BaseTurretData,
                DifficultMultiplier = _waveModifier
            });

            _ultimate?.Initialize(shipConfig.BaseTurretData.ProjectileData.Damage);
        }

        public void SetWaveModifier(float waveModifier)
        {
            _waveModifier = waveModifier;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _health);
        }

        public void Restore()
        {
            _currentHealth = _health;
        }
    }
}