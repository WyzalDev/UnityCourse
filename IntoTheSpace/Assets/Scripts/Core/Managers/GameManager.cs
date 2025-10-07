// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private UltimateManager _ultimateManager;
        [SerializeField] private ProjectilesManager _projectilesManager;
        [SerializeField] private HUDManager _hudManager;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _unpauseButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private List<GameObject> _controllables;
        [SerializeField] private Ship _playerShipPrefab;
        [SerializeField] private Transform _playerSpawnPoint;

        private List<IPausable> _pausables;
        private List<IRestorable> _restorables;
        private Ship _playerShip;

        private void Start()
        {
            _enemyManager.OnEnemyReachedEndPath += EndGame;
            _pauseButton.onClick.AddListener(PauseGame);
            _unpauseButton.onClick.AddListener(UnpauseGame);
            _restartButton.onClick.AddListener(RestartGame);

            _pausables = new List<IPausable>();
            _restorables = new List<IRestorable>();

            SpawnPlayer();

            foreach (var controllabe in _controllables)
            {
                var pausables = controllabe.GetComponents<IPausable>();
                if (pausables is not null && pausables.Length > 0)
                    _pausables.AddRange(pausables);

                var restorables = controllabe.GetComponents<IRestorable>();
                if (restorables is not null && restorables.Length > 0)
                    _restorables.AddRange(restorables);
            }
        }

        private void RestartGame()
        {
            RestoreGame();

            if (_playerShip is null)
                SpawnPlayer();

            UnpauseGame();
        }

        private void SpawnPlayer()
        {
            _playerShip = Instantiate(_playerShipPrefab, _playerSpawnPoint);
            _playerShip.SetWaveModifier(1f);
            _restorables.Add(_playerShip);
            _playerShip.OnShipDestroyed += OnPlayerDeath;
            if (_playerShip?.Ultimate is not Ultimate ultimate)
                return;

            _hudManager.SetNewPlayer(_playerShip);
            _ultimateManager.SetNewPlayerUltimate(ultimate);
            _enemyManager.OnEnemyHitted += ultimate.AddPercentage;
        }

        private void PauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = true;

            Time.timeScale = 0;

            _unpauseButton.interactable = true;
            _pauseButton.interactable = false;
        }

        private void UnpauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = false;

            Time.timeScale = 1f;

            _unpauseButton.interactable = false;
            _pauseButton.interactable = true;
        }

        private void RestoreGame()
        {
            foreach (var restorable in _restorables)
                restorable.Restore();
        }

        private void EndGame()
        {
            PauseGame();
            DestroyPlayer();
        }

        private void OnPlayerDeath(Ship playerShip, int health)
        {
            EndGame();
        }

        private void DestroyPlayer(bool isDestroyed = false)
        {
            if (_playerShip is null)
                return;

            if (_playerShip.Ultimate is Ultimate ultimate)
            {
                _enemyManager.OnEnemyHitted -= ultimate.AddPercentage;
                _hudManager.SetNewPlayer(null);
            }

            _ultimateManager.OnPlayerDestroyed();
            _restorables.Remove(_playerShip);
            _playerShip.OnShipDestroyed -= OnPlayerDeath;

            if(isDestroyed)
                return;

            Destroy(_playerShip.gameObject);
            _playerShip = null;
        }

        private void OnDestroy()
        {
            _enemyManager.OnEnemyReachedEndPath -= EndGame;
            _pauseButton.onClick.RemoveListener(PauseGame);
            _unpauseButton.onClick.RemoveListener(UnpauseGame);
            _restartButton.onClick.RemoveListener(RestartGame);
            DestroyPlayer(true);
        }
    }
}