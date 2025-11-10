// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class LevelConfig
    {
        [SerializeField] private GoalsConfig _goalsConfig;
        [SerializeField] private int _movesLimitation;
        [SerializeField] private int _seed;
        [SerializeField] private FieldConfig _fieldConfig;

        public GoalsConfig GoalsConfig => _goalsConfig;

        public int MovesLimitation => _movesLimitation;

        public int Seed => _seed;

        public FieldConfig FieldConfig => _fieldConfig;
    }
}