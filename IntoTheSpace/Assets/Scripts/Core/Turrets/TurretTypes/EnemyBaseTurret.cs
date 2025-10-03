// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Turrets.TurretTypes
{
    public class EnemyBaseTurret : BaseTurret
    {
        [SerializeField] private EnemyVision _vision;

        protected override void Start()
        {
            _vision.OnPlayerInSight += StartFire;
            _vision.OnPlayerOutSight += StopFire;
        }

        protected override void OnDestroy()
        {
            _vision.OnPlayerInSight -= StartFire;
            _vision.OnPlayerOutSight -= StopFire;
        }
    }
}