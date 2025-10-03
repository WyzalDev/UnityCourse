// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Data.TurretTypes;
using Core.Managers;

namespace Core.Turrets.TurretTypes
{
    public class PlayerTripleTurret : PlayerBaseTurret
    {
        private Vector2 _firePointDisplacement;
        private float _fireAngle;

        protected override void OnConfigUpdated()
        {
            base.OnConfigUpdated();

            if (BaseTurretConfig is not TripleTurretConfig tripleTurretScriptable)
                return;

            _firePointDisplacement = tripleTurretScriptable.FirePointDisplacement;
            _fireAngle = tripleTurretScriptable.FireAngle;
        }

        protected override void Fire()
        {
            ProjectilesManager.InstantiateProjectile(_projectileData, _turretFirePoint.position, Quaternion.identity);

            ProjectilesManager.InstantiateProjectile(_projectileData,
                _turretFirePoint.position + (Vector3)_firePointDisplacement, Quaternion.Euler(0, 0, -_fireAngle));

            ProjectilesManager.InstantiateProjectile(_projectileData,
                _turretFirePoint.position - (Vector3)_firePointDisplacement, Quaternion.Euler(0, 0, _fireAngle));
        }
    }
}