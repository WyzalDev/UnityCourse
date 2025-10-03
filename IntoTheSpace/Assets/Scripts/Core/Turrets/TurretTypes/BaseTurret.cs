// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Managers;

namespace Core.Turrets.TurretTypes
{
    public abstract class BaseTurret : Turret
    {
        protected override void Fire()
        {
            ProjectilesManager.InstantiateProjectile(_projectileData, _turretFirePoint.position, transform.rotation);
        }
    }
}