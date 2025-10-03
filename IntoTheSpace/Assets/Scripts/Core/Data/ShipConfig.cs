// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Data.TurretTypes;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "NewShipData", menuName = "Gameplay/ShipData", order = 0)]
    public class ShipConfig : ScriptableObject
    {
        [Header("Main Config")]
        [SerializeField] private string _name;
        [SerializeField] private int _health;
        [SerializeField] private float _speed;

        [Header("Fire Config")]
        [SerializeField] private BaseTurretConfig _baseTurretData;

        public string Name => _name;
        public int Health => _health;
        public float Speed => _speed;
        public BaseTurretConfig BaseTurretData => _baseTurretData;
    }
}