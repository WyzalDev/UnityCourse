// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Data.TurretTypes
{
    [CreateAssetMenu(fileName = "NewTripleTurretTypeConfig", menuName = "Gameplay/TurretTypes/Triple", order = 0)]
    public class TripleTurretConfig : BaseTurretConfig
    {
        [Header("Fire Additional Settings")]
        [SerializeField] private Vector2 _firePointDisplacement;
        [SerializeField] private float _fireAngle;

        public Vector2 FirePointDisplacement => _firePointDisplacement;
        public float FireAngle => _fireAngle;
    }
}