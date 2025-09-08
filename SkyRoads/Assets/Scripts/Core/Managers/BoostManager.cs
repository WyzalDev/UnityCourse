// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Data;

namespace Core.Managers
{
    public class BoostManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _controllables;

        [Header("Boost Settings")]
        [SerializeField] private BoostData _startBoostData;
        [SerializeField] private BoostData _endBoostData;

        private InputAction _speedBoostAction;
        private List<IBoostable> _boostables;

        private void Start()
        {
            _speedBoostAction = InputSystem.actions.FindAction("SpeedBoost");
            _speedBoostAction.started += OnStarted;
            _speedBoostAction.performed += OnPerformed;

            _boostables = new List<IBoostable>();

            foreach (var controllable in _controllables)
            {
                if (controllable.TryGetComponent<IBoostable>(out var boostable))
                    _boostables.Add(boostable);
            }
        }

        private void OnStarted(InputAction.CallbackContext context)
        {
            foreach (var boostable in _boostables)
                boostable.OnBoostStart(_startBoostData);
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            foreach (var boostable in _boostables)
                boostable.OnBoostEnd(_endBoostData);
        }

        private void OnDestroy()
        {
            _speedBoostAction.started -= OnStarted;
            _speedBoostAction.performed -= OnPerformed;
        }
    }
}