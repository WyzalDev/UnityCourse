// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class GoalsConfig
    {
        [SerializeField] private int _requiredValue;
        [SerializeField] private GoalType _goalType;
        [SerializeField] private int _scoreMultiplier;
        [SerializeField] private ElementType _goalElementType;

        public GoalsConfig(int requiredValue, GoalType goalType, int scoreMultiplier, ElementType goalElementType)
        {
            _requiredValue = requiredValue;
            _goalType = goalType;
            _scoreMultiplier = scoreMultiplier;
            _goalElementType = goalElementType;
        }

        public GoalsConfig()
        {
        }

        public int RequiredValue => _requiredValue;

        public GoalType GoalType => _goalType;

        public int ScoreMultiplier => _scoreMultiplier;

        public ElementType GoalElementType => _goalElementType;
    }
}