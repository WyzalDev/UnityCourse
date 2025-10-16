// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Data.TurretTypes
{
    [CreateAssetMenu(fileName = "NewBaseTurretTypeConfig", menuName = "Gameplay/TurretTypes/Base", order = 0)]
    public class BaseTurretConfig : ScriptableObject
    {
        [SerializeField] private string _description;

        [Header("Fire Settings")]
        [SerializeField] private float _fireRate;

        [SerializeField] private string _fireSoundName;

        [Header("Projectile Settings")]
        [SerializeField] private ProjectileData _projectileData;

        public string Description => _description;
        public float FireRate => _fireRate;
        public string FireSoundName => _fireSoundName;
        public ProjectileData ProjectileData => _projectileData;
    }
}