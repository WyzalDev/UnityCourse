// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using UI.Managers;

namespace UI.Views.Menu
{
    public class MainMenuPage : Page
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        private Action _comicAction;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            PageManager.Show<ChooseLevelPage>();
        }

        private void OnSettingsButtonClicked()
        {
            PageManager.Show<SettingsPage>();
        }

        private void OnExitButtonClicked()
        {
            PopupManager.ShowPopup<ExitPopup>();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }
    }
}