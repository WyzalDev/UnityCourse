// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "NewUltimateConfig", menuName = "Gameplay/Ultimate", order = 0)]
    public class UltimateConfig : ScriptableObject
    {
        [SerializeField] [Range(0.001f, 1)] private float _percentPerHit;
        [SerializeField] private float _damagePercent;

        public float PercentPerHit => _percentPerHit;
        public float DamagePercent => _damagePercent;
    }
}