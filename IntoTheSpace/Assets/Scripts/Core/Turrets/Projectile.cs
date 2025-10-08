// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.Data;
using Core.Managers;

namespace Core.Turrets
{
    [Serializable]
    public class Projectile : MonoBehaviour, IInitializable
    {
        [SerializeField] private ParticleSystem _hitEffect;

        private Rigidbody2D _rigidbody;
        private float _speed;
        private int _damage;
        private bool _isPaused;
        private float _currentSpeed;
        private float _lifeTime;

        public float CurrentSpeed
        {
            get => _currentSpeed;
            private set
            {
                _currentSpeed = value;
                if (gameObject.activeSelf)
                    _rigidbody.AddRelativeForceY(value);
            }
        }

        public void Initialize(object data)
        {
            if (data is not ProjectileData projectileData)
                return;

            _rigidbody = GetComponent<Rigidbody2D>();
            _speed = projectileData.Speed;
            _damage = projectileData.Damage;
            _lifeTime = projectileData.LifeTime;
            CurrentSpeed = _speed;
        }

        private void FixedUpdate()
        {
            _lifeTime = Mathf.Clamp(_lifeTime - Time.fixedDeltaTime, 0, float.MaxValue);
            if (_lifeTime == 0)
                ProjectilesManager.DestroyProjectile(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.TryGetComponent<IDamageable>(out var damageable))
                return;

            Instantiate(_hitEffect, transform.position, transform.rotation);
            damageable.TakeDamage(_damage);
            ProjectilesManager.DestroyProjectile(this, other.collider.CompareTag("Enemy"));
        }
    }
}