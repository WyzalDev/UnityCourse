// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.RoadObjects;

namespace Core.MovingContainer
{
    public class Player : MonoBehaviour, IRestorable
    {
        [SerializeField] private GameObject _playerDeathEffect;
        [SerializeField] private float _playerDeathEffectDuration;

        public Action PlayerCrashed;

        private void InvokePlayerCrashed()
        {
            PlayerCrashed?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Asteroid>(out var asteroid))
                return;

            InvokePlayerCrashed();

            var effect = Instantiate(_playerDeathEffect, transform.position, transform.rotation);
            Destroy(effect.gameObject, _playerDeathEffectDuration);

            gameObject.SetActive(false);
        }

        public void Restore()
        {
            gameObject.SetActive(true);
        }
    }
}