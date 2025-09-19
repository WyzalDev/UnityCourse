// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Audio.Managers;
using UI.Data;
using UI.Managers;

namespace UI.Views.Game
{
    public class WinPage : Page
    {
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backToMenuButton;

        [Header("Fireworks Settings")]
        [SerializeField] private ParticleSystem _fireworkPrefab;
        [SerializeField] private float _fireworkLifetime;
        [SerializeField] private Transform _firstFireworkTransform;
        [SerializeField] private Transform _secondFireworkTransform;

        [Header("Popup Text Settings")]
        [SerializeField] private PopupAnimation _popupText;
        [SerializeField] private RectTransform _popupTransform;

        private Action _restartAction;
        private Action _backToMenuAction;
        private ParticleSystem _firstFireWork;
        private ParticleSystem _secondFireWork;

        private void Start()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        public override void Show(object data = null)
        {
            if (data is WinLoseData winLoseData)
            {
                _score.text = winLoseData.Score.ToString();
                _restartAction = winLoseData.ButtonAction;
                _backToMenuAction = winLoseData.BackToMenuAction;
            }

            base.Show(data);

            Instantiate(_popupText, _popupTransform);
            InstantiateFireworks();
            
            AudioManager.PlaySfx("WinSound");
            AudioManager.PlaySfx("GameOver");
        }

        private void InstantiateFireworks()
        {
            _firstFireWork = Instantiate(_fireworkPrefab, _firstFireworkTransform);
            _secondFireWork = Instantiate(_fireworkPrefab, _secondFireworkTransform);

            _firstFireWork.Play();
            _secondFireWork.Play();

            Destroy(_firstFireWork.gameObject, _fireworkLifetime);
            Destroy(_secondFireWork.gameObject, _fireworkLifetime);
        }

        public override void Hide()
        {
            base.Hide();
            if (_firstFireWork != null)
                _firstFireWork.gameObject.SetActive(false);

            if (_secondFireWork != null)
                _secondFireWork.gameObject.SetActive(false);
        }

        private void OnRestartButtonClicked()
        {
            PageManager.Last();
            _restartAction?.Invoke();
        }

        private void OnBackToMenuButtonClicked()
        {
            _backToMenuAction?.Invoke();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
        }
    }
}