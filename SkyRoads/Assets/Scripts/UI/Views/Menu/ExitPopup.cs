// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using UI.Managers;

namespace UI.Views.Menu
{
    public class ExitPopup : Popup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _backButton;

        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        private void OnBackButtonClicked()
        {
            PopupManager.HidePopup();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}