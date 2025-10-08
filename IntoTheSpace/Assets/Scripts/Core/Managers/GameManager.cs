// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Data;
using Core.Records;
using UI.Data;
using UI.Managers;
using UI.Views.Game;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private UltimateManager _ultimateManager;
        [SerializeField] private ProjectilesManager _projectilesManager;
        [SerializeField] private HUDManager _hudManager;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private List<GameObject> _controllables;
        [SerializeField] private Ship _playerShipPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private float _endGameUIDelay;

        private List<IPausable> _pausables;
        private List<IRestorable> _restorables;
        private Ship _playerShip;
        private WaitForSecondsRealtime _cachedEndGameUIDelay;

        private void Start()
        {
            _enemyManager.OnEnemyReachedEndPath += EndGame;
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);

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

            _cachedEndGameUIDelay = new WaitForSecondsRealtime(_endGameUIDelay);
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

        private void OnPauseButtonClicked()
        {
            PageManager.Show<PausePage>(new PauseData()
            {
                ButtonAction = UnpauseGame,
                BackToMenuAction = BackToMenu
            });

            PauseGame();
        }

        private void PauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = true;

            Time.timeScale = 0;

            _pauseButton.interactable = false;
        }

        private void UnpauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = false;

            Time.timeScale = 1f;

            _pauseButton.interactable = true;
        }

        private void BackToMenu()
        {
            //TODO load menu scene when it would be implemented
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

            StartCoroutine(ShowEndGameUI());
        }

        private IEnumerator ShowEndGameUI()
        {
            yield return _cachedEndGameUIDelay;

            var scoreInfo = new RecordInfo(Score.Value, DateTime.Now);
            var pageData = new WinLoseData()
            {
                Score = Score.Value,
                ButtonAction = RestartGame,
                BackToMenuAction = BackToMenu
            };

            if (ScoreRecords.TryAdd(scoreInfo))
                PageManager.Show<WinPage>(pageData);
            else
                PageManager.Show<LoosePage>(pageData);
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

            if (isDestroyed)
                return;

            Destroy(_playerShip.gameObject);
            _playerShip = null;
        }

        private void OnDestroy()
        {
            _enemyManager.OnEnemyReachedEndPath -= EndGame;
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
            DestroyPlayer(true);
        }
    }
}