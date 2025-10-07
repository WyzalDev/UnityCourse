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
        public Action OnShipInitialized;

        private float _waveModifier;

        public Action OnDamageTaken { get; set; }

        public IInitializable Movement { get; private set; }
        public IInitializable Turret { get; private set; }
        public IInitializable Ultimate { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<Movement>();
            Turret = GetComponent<Turret>();
            if (TryGetComponent<Ultimate>(out var ultimate))
                Ultimate = ultimate;
        }

        private void Start()
        {
            MaxHealth = Mathf.RoundToInt(shipConfig.Health * _waveModifier);
            Restore();

            Movement.Initialize(shipConfig.Speed);

            Turret.Initialize(new TurretData()
            {
                turretConfig = shipConfig.BaseTurretData,
                DifficultMultiplier = _waveModifier
            });

            Ultimate?.Initialize(shipConfig.BaseTurretData.ProjectileData.Damage);
            OnShipInitialized?.Invoke();
        }

        public void SetWaveModifier(float waveModifier)
        {
            _waveModifier = waveModifier;
        }

        public void TakeDamage(int damage)
        {
            var formerHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

            if (formerHealth > CurrentHealth)
                OnDamageTaken?.Invoke();

            if (CurrentHealth == 0)
                OnShipDestroyed?.Invoke(this, MaxHealth);
        }

        public void Restore()
        {
            CurrentHealth = MaxHealth;
        }
    }
}