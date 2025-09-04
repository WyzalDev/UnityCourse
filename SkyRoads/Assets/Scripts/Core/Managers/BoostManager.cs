// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Data;

namespace Core.Managers
{
    public class BoostManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _boostables;

        [Header("Boost Settings")] 
        [SerializeField] private BoostData _startBoostData;
        [SerializeField] private BoostData _endBoostData;

        private InputAction _speedBoostAction;

        private void Start()
        {
            _speedBoostAction = InputSystem.actions.FindAction("SpeedBoost");
            _speedBoostAction.started += OnStarted;
            _speedBoostAction.performed += OnPerformed;
        }

        private void OnStarted(InputAction.CallbackContext context)
        {
            foreach (var boostable in _boostables)
            {
                if (boostable.TryGetComponent<IBoostable>(out var component))
                    component.OnBoostStart(_startBoostData);
            }
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            foreach (var boostable in _boostables)
            {
                if (boostable.TryGetComponent<IBoostable>(out var component))
                    component.OnBoostEnd(_endBoostData);
            }
        }
        
        private void OnDestroy()
        {
            _speedBoostAction.started -= OnStarted;
            _speedBoostAction.performed -= OnPerformed;
        }
    }
}