// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "NewWavesConfig", menuName = "Gameplay/WavesConfig", order = 0)]
    public class WavesConfig : ScriptableObject
    {
        [SerializeField] private List<int> _firstUniqueWavesEnemyCount;
        [SerializeField] private List<int> _cycleWavesEnemyCount;
        [SerializeField] private float _spawnTime;

        [Tooltip("0 means no game difficulty growth per wave, 0.2 means 20% difficulty growth per wave")]
        [SerializeField] [Range(0, 1)] private float _difficultyGrowthPerWave;
        [Tooltip("0.001f means too small game difficulty at start, 0.2 means 20% game difficulty at start")]
        [SerializeField] [Range(0.001f, 5f)] private float _startGameDifficulty;

        public List<int> FirstUniqueWavesEnemyCount => _firstUniqueWavesEnemyCount;
        public List<int> CycleWavesEnemyCount => _cycleWavesEnemyCount;
        public float SpawnTime => _spawnTime;
        public float DifficultyGrowthPerWave => _difficultyGrowthPerWave;
        public float StartGameDifficulty => _startGameDifficulty;
    }
}