// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Audio.Managers;
using Core.Data;

namespace Core
{
    public class Ultimate : MonoBehaviour, IInitializable, IRestorable
    {
        [SerializeField] private UltimateConfig _ultimateConfig;

        public Action<float> OnUltimateUsed;
        public Action OnUltimateReady;
        public Action<float> OnUltimateFill;

        private InputAction _ultimateAction;
        private float _percentPerHit;
        private float _baseDamage;
        private float _ultimateMultiplier;
        private float _percentage;
        private bool _isReady;
        private bool _isReadySoundPlayed;

        private const string UltimateReadySoundName = "UltimateReady";
        private const string UltimateUsedSoundName = "UltimateUsed";

        private void Start()
        {
            _ultimateAction = InputSystem.actions.FindAction("Ultimate");
            _ultimateAction.performed += Use;
            _percentPerHit = _ultimateConfig.PercentPerHit;
            _ultimateMultiplier = _ultimateConfig.DamagePercent;
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

            if(_percentage != 0 && !Mathf.Approximately(_percentage, 1f))
                OnUltimateFill?.Invoke(_percentage);

            if (!_isReady)
                return;

            OnUltimateReady?.Invoke();
            
            if(_isReadySoundPlayed)
                return;

            AudioManager.PlaySfx(UltimateReadySoundName);
            _isReadySoundPlayed = true;
        }

        private void Use(InputAction.CallbackContext context)
        {
            if (!_isReady)
                return;

            AudioManager.PlaySfx(UltimateUsedSoundName);
            Restore();
            OnUltimateUsed?.Invoke(_baseDamage * _ultimateMultiplier);
        }

        public void Restore()
        {
            _percentage = 0;
            _isReady = false;
            _isReadySoundPlayed = false;
        }

        private void OnDestroy()
        {
            _ultimateAction.performed -= Use;
        }
    }
}