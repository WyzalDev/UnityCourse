// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine.InputSystem;

namespace Core.Turrets.TurretTypes
{
    public class PlayerBaseTurret : BaseTurret
    {
        private InputAction _fireAction;

        protected override void Start()
        {
            _fireAction = InputSystem.actions.FindAction("Fire");
            _fireAction.performed += StartFire;
            _fireAction.canceled += StopFire;
        }

        private void StartFire(InputAction.CallbackContext context)
        {
            base.StartFire();
        }

        private void StopFire(InputAction.CallbackContext context)
        {
            base.StopFire();
        }

        protected override void OnDestroy()
        {
            _fireAction.performed -= StartFire;
            _fireAction.canceled -= StopFire;
        }
    }
}