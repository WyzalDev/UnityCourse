// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Utility;
using Core.RoadObjects;
using Core.Data;

namespace Core.Managers
{
    public class AsteroidSpawner : MonoBehaviour, IBoostable, IPausable, IRestorable
    {
        [SerializeField] private Transform _asteroidParent;
        [SerializeField] private Asteroid _asteroidPrefab;

        [Header("Spawn settings")]
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private int _startAsteroidsCount;
        [SerializeField] private float _startSpawnRate = 1f;
        [SerializeField] private float _spawnGrowthRate;
        [Tooltip("When growth rate reaches threshold, then threshold value will be used instead of offset value")]
        [SerializeField] private float _spawnGrowthRateThreshold;

        public bool IsPaused { get; set; }

        private CircularList<Asteroid> _asteroids = new();
        private float _timeToNextSpawn;
        private float _currentBoostMultiplier = 1f;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var asteroids = new List<Asteroid>();

            for (var i = 0; i < _startAsteroidsCount; i++)
            {
                var asteroid = Instantiate(_asteroidPrefab, _asteroidParent);
                asteroid.gameObject.SetActive(false);
                asteroids.Add(asteroid);
            }

            _asteroids.Initialize(asteroids);
        }

        public void OnBoostStart(BoostData startParameters)
        {
            ChangeBoostMultiplier(startParameters.AsteroidSpawnRateMultiplier);
        }

        public void OnBoostEnd(BoostData endParameters)
        {
            ChangeBoostMultiplier(endParameters.AsteroidSpawnRateMultiplier);
        }

        private void ChangeBoostMultiplier(float boostMultiplier)
        {
            _currentBoostMultiplier = boostMultiplier > 1f ? boostMultiplier : 1f;
        }

        public void Restore()
        {
            var asteroids = _asteroids.RemoveAll();

            foreach (var asteroid in asteroids)
                Destroy(asteroid.gameObject);

            Initialize();
            _timeToNextSpawn = 0;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            if (_timeToNextSpawn == 0)
            {
                SpawnAsteroid();
                _timeToNextSpawn = _startSpawnRate;
            }

            var offset = _spawnGrowthRate * Time.deltaTime * Timer.GameDifficultMultiplier;

            _timeToNextSpawn = Mathf.Clamp(
                _timeToNextSpawn -
                (offset < _spawnGrowthRateThreshold ? offset : _spawnGrowthRateThreshold) * _currentBoostMultiplier,
                0, _startSpawnRate);
        }

        private void SpawnAsteroid()
        {
            var nextAsteroid = _asteroids.Next();

            if (nextAsteroid.gameObject.activeSelf)
            {
                var newAsteroid = Instantiate(_asteroidPrefab, _asteroidParent);
                _asteroids.Add(newAsteroid);
                newAsteroid.gameObject.SetActive(true);
                newAsteroid.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
            }
            else
            {
                nextAsteroid.gameObject.SetActive(true);
                nextAsteroid.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
            }
        }
    }
}