// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Data;
using Core.Movements;

namespace Core.Managers
{
    public class EnemyManager : MonoBehaviour, IPausable, IRestorable
    {
        [Header("Spawn Settings")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Ship _enemyPrefab;
        [SerializeField] private Transform _enemyShipsParent;

        [Header("Waves Settings")]
        [SerializeField] private WavesConfig _wavesConfig;
        [SerializeField] private List<DestinationPoint> _movePoints;

        public Action OnEnemyReachedEndPath;
        public Action OnEnemyHitted;

        private List<Ship> _enemyShips;
        private float _difficultyGrowthPerWave;
        private float _currentGameDifficulty;
        private float _spawnTime;
        private float _spawnCooldown;
        private int _currentEnemiesCount;
        private int _currentWave = -1;
        private int _enemyWaitingForSpawn;
        private bool _isSpawning;

        public bool IsPaused { get; set; }

        private int CurrentEnemiesCount
        {
            get => _currentEnemiesCount;
            set
            {
                _currentEnemiesCount = value;
                if (value == 0)
                    OnNewWave();
            }
        }

        private int EnemyWaitingForSpawn
        {
            get => _enemyWaitingForSpawn;
            set
            {
                _enemyWaitingForSpawn = value;
                _isSpawning = value != 0;
            }
        }

        private void Start()
        {
            if (_wavesConfig == null ||
                _wavesConfig.FirstUniqueWavesEnemyCount.Count + _wavesConfig.CycleWavesEnemyCount.Count <= 0)
                return;

            _enemyShips = new List<Ship>();

            _difficultyGrowthPerWave = _wavesConfig.DifficultyGrowthPerWave;
            _spawnTime = _wavesConfig.SpawnTime;
            _currentGameDifficulty = _wavesConfig.StartGameDifficulty - _difficultyGrowthPerWave;
            OnNewWave();
        }

        public void AllEnemiesTakeDamage(float damage)
        {
            for (var i = 0; i < _enemyShips.Count; i++)
                _enemyShips[i].TakeDamage(Mathf.CeilToInt(damage));
        }

        private void OnNewWave()
        {
            _currentWave++;
            _currentGameDifficulty += _difficultyGrowthPerWave;

            if (_currentWave < _wavesConfig.FirstUniqueWavesEnemyCount.Count)
                EnemyWaitingForSpawn = _wavesConfig.FirstUniqueWavesEnemyCount[_currentWave];
            else if (_wavesConfig.CycleWavesEnemyCount.Count != 0)
            {
                var rightIndex = (_currentWave - _wavesConfig.FirstUniqueWavesEnemyCount.Count) %
                                 _wavesConfig.CycleWavesEnemyCount.Count;
                EnemyWaitingForSpawn = _wavesConfig.CycleWavesEnemyCount[rightIndex];
            }
        }

        private void FixedUpdate()
        {
            if (IsPaused || !_isSpawning)
                return;

            _spawnCooldown = Mathf.Clamp(_spawnCooldown - Time.fixedDeltaTime, 0, _spawnTime);

            if (_spawnCooldown != 0)
                return;

            InstantiateEnemy();
            _spawnCooldown = _spawnTime;
        }

        private void InstantiateEnemy()
        {
            var enemyShip = Instantiate(_enemyPrefab, _spawnPoint.position, _enemyPrefab.transform.rotation,
                _enemyShipsParent);

            _enemyShips.Add(enemyShip);
            enemyShip.OnShipDestroyed += OnEnemyDeath;
            enemyShip.SetWaveModifier(_currentGameDifficulty);
            EnemyWaitingForSpawn--;
            CurrentEnemiesCount++;

            if (_movePoints is null || _movePoints.Count <= 0)
                return;

            if (enemyShip.Movement is not EnemyMovement enemyMovement)
                return;

            enemyMovement.SetPath(_movePoints);
            if(OnEnemyReachedEndPath is not null)
                enemyMovement.OnPathEnded += OnEnemyReachedEndPath.Invoke;
        }

        private void OnEnemyDeath(Ship enemyShip, int health)
        {
            enemyShip.OnShipDestroyed -= OnEnemyDeath;
            Score.AddScore(health);

            _enemyShips.Remove(enemyShip);
            CurrentEnemiesCount--;

            if (enemyShip.Movement is EnemyMovement enemyMovement && OnEnemyReachedEndPath is not null)
                enemyMovement.OnPathEnded -= OnEnemyReachedEndPath.Invoke;

            Destroy(enemyShip.gameObject);
        }

        public void Restore()
        {
            _currentWave = -1;
            _enemyWaitingForSpawn = 0;
            _spawnCooldown = 0;
            CurrentEnemiesCount = 0;
            _currentGameDifficulty = _wavesConfig.StartGameDifficulty;

            foreach (var enemyShip in _enemyShips)
                Destroy(enemyShip.gameObject);

            _enemyShips.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;

            if (_movePoints is null || _movePoints.Count <= 0)
                return;

            Gizmos.DrawSphere(_movePoints[0].Position, 0.1f);

            if (_movePoints.Count <= 1)
                return;

            for (var i = 1; i < _movePoints.Count; i++)
            {
                Gizmos.DrawLine(_movePoints[i - 1].Position, _movePoints[i].Position);
                Gizmos.DrawSphere(_movePoints[i].Position, 0.1f);
            }
        }
#endif
    }
}