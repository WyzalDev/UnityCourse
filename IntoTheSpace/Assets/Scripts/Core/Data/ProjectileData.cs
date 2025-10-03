// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.Turrets;

namespace Core.Data
{
    [Serializable]
    public class ProjectileData
    {
        [SerializeField] public Projectile Prefab;
        [SerializeField] public float Speed;
        [SerializeField] public float LifeTime;
        [SerializeField] public int Damage;
    }
}