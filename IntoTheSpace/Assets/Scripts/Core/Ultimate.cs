// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Data;

namespace Core
{
    public class Ultimate : MonoBehaviour, IInitializable, IRestorable
    {
        [SerializeField] UltimateConfig ultimateConfig;

        public Action<float> OnUltimateUsed;

        private InputAction _ultimateAction;
        private float _percentPerHit;
        private float _baseDamage;
        private float _ultimateMultiplier;
        private float _percentage;
        private bool _isReady;

        private void Start()
        {
            _ultimateAction = InputSystem.actions.FindAction("Ultimate");
            _ultimateAction.performed += Use;
            _percentPerHit = ultimateConfig.PercentPerHit;
            _ultimateMultiplier = ultimateConfig.DamagePercent;
        }

        public void Initialize(object data)
        {
            if (data is int damage)
                _baseDamage = damage;
        }

        public void AddPercentage()
        {
            _percentage = Mathf.Clamp(_percentage + _percentPerHit, 0, 1f);
            _isReady = Mathf.Approximately(_percentage, 1f);

            //TODO: delete
            if (_isReady)
                Debug.Log("Ultimate Ready");
        }

        private void Use(InputAction.CallbackContext context)
        {
            if (!_isReady)
                return;

            Restore();
            OnUltimateUsed?.Invoke(_baseDamage * _ultimateMultiplier);
            Debug.Log("Ultimate Used");
        }

        public void Restore()
        {
            _percentage = 0;
            _isReady = false;
        }

        private void OnDestroy()
        {
            _ultimateAction.performed -= Use;
        }
    }
}