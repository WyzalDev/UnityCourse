// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.Data.TurretTypes;

namespace Core.Data
{
    [Serializable]
    public class TurretData
    {
        [SerializeField] public BaseTurretConfig turretConfig;
        [SerializeField] public float DifficultMultiplier;
    }
}