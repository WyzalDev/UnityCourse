// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using UI.Data;
using UI.Managers;

namespace UI.Views.Game
{
    public class WinPage : Page
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backToMenuButton;

        private Action _restartAction;
        private Action _nextLevelAction;

        private void Start()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        public override void Show(object data = null)
        {
            if (data is GamePageData winLoseData)
            {
                _restartAction = winLoseData.ButtonAction;
                _nextLevelAction = winLoseData.BackToMenuAction;
            }

            base.Show(data);
        }

        private void OnRestartButtonClicked()
        {
            PageManager.Last();
            _restartAction?.Invoke();
        }

        private void OnBackToMenuButtonClicked()
        {
            _nextLevelAction?.Invoke();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
        }
    }
}