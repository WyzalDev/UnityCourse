// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
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

        [SerializeField] private List<GameObject> _controlableObjects;
        
        private void Start()
        {
            _player.PlayerCrashed += OnEndGame;
        }
        
        public void RestartGame()
        {
            Restore();

            foreach (var possiblePausable in _controlableObjects)
            {
                if (possiblePausable.TryGetComponent<IPausable>(out var component))
                    component.IsPaused = false;
            }
        }

        private void Restore()
        {
            foreach (var possibleRestorable in _controlableObjects)
            {
                if (possibleRestorable.TryGetComponent<IRestorable>(out var component))
                    component.Restore();
            }
        }

        private void OnEndGame()
        {
            foreach (var possiblePausable in _controlableObjects)
            {
                if (possiblePausable.TryGetComponent<IPausable>(out var component))
                    component.IsPaused = true;
            }

            var scoreInfo = new RecordInfo(_scoreManager.Score, DateTime.Now, Timer.Value);

            if (ScoreRecords.TryAdd(scoreInfo))
                PageManager.Show<WinPage>(_scoreManager.Score);
            else
                PageManager.Show<LoosePage>(_scoreManager.Score);
        }

        private void OnDestroy()
        {
            _player.PlayerCrashed -= OnEndGame;
        }
    }
}