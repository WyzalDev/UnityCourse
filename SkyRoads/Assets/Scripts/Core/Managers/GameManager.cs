// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;
using UI.Views;
using Core.Data;
using Core.MovingContainer;
using Core.Records;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private Player _player;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private float _endGameUIDelay;

        [SerializeField] private List<GameObject> _controllableObjects;

        private List<IPausable> _pausableObjects;
        private List<IRestorable> _restorableObjects;

        private void Start()
        {
            _player.PlayerCrashed += OnEndGame;
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);

            _pausableObjects = new List<IPausable>();
            _restorableObjects = new List<IRestorable>();

            foreach (var controllable in _controllableObjects)
            {
                if (controllable.TryGetComponent<IPausable>(out var pausable))
                    _pausableObjects.Add(pausable);

                if (controllable.TryGetComponent<IRestorable>(out var restorable))
                    _restorableObjects.Add(restorable);
            }
        }

        public void RestartGame()
        {
            Restore();
            UnpauseGame();
        }

        private void PauseGame()
        {
            foreach (var pausable in _pausableObjects)
                pausable.IsPaused = true;
            
            _pauseButton.interactable = false;
        }

        private void UnpauseGame()
        {
            foreach (var pausable in _pausableObjects)
                pausable.IsPaused = false;
            
            _pauseButton.interactable = true;
        }

        private void OnPauseButtonClicked()
        {
            PageManager.Show<PausePage>(new Action(UnpauseGame));
            PauseGame();
        }

        private void Restore()
        {
            foreach (var restorable in _restorableObjects)
                restorable.Restore();
        }

        private void OnEndGame()
        {
            PauseGame();
            Invoke(nameof(ShowEndGameUI), _endGameUIDelay);
        }

        private void ShowEndGameUI()
        {
            var scoreInfo = new RecordInfo(_scoreManager.Score, DateTime.Now, Timer.Value);
            var pageData = new PageData()
            {
                Score = _scoreManager.Score,
                Action = RestartGame
            };

            if (ScoreRecords.TryAdd(scoreInfo))
                PageManager.Show<WinPage>(pageData);
            else
                PageManager.Show<LoosePage>(pageData);
        }

        private void OnDestroy()
        {
            _player.PlayerCrashed -= OnEndGame;
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
        }
    }
}