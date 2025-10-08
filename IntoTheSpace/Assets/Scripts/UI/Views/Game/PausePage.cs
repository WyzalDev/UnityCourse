// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;
using UI.Data;
using UI.Managers;

namespace UI.Views.Game
{
    public class PausePage : Page
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _sfxToggle;
        [SerializeField] private Button _backToGameButton;
        [SerializeField] private Button _backToMenuButton;

        [Header("Fade Settings")]
        [SerializeField] private Image _fade;
        [SerializeField] private float _maxFadeValue;
        [SerializeField] private float _fadeDuration;

        private Action _unpauseAction;
        private Action _backToMenuAction;

        private void Start()
        {
            _musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            _sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
            _backToGameButton.onClick.AddListener(OnBackToGameButtonClicked);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        public override void Show(object data = null)
        {
            if (data is PauseData pauseData)
            {
                _unpauseAction = pauseData.ButtonAction;
                _backToMenuAction = pauseData.BackToMenuAction;
            }

            CorrectToggles();

            _fade.DOFade(_maxFadeValue, _fadeDuration)
                .SetEase(Ease.InQuart)
                .SetUpdate(true)
                .OnComplete(() => base.Show(data));
        }

        private void CorrectToggles()
        {
            _audioMixerGroup.audioMixer.GetFloat("MusicVolume", out var musicVolume);
            _audioMixerGroup.audioMixer.GetFloat("SFXVolume", out var sfxVolume);

            if (musicVolume == 0f)
                _musicToggle.isOn = true;
            else if (Mathf.Approximately(musicVolume, -80f))
                _musicToggle.isOn = false;

            if (sfxVolume == 0f)
                _sfxToggle.isOn = true;
            else if (Mathf.Approximately(sfxVolume, -80f))
                _sfxToggle.isOn = false;
        }

        public override void Hide()
        {
            base.Hide();

            _fade.color = new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f);
        }

        private void OnMusicToggleChanged(bool isOn)
        {
            var value = isOn ? 0f : -80f;

            _audioMixerGroup.audioMixer.SetFloat("MusicVolume", value);
        }

        private void OnSfxToggleChanged(bool isOn)
        {
            var value = isOn ? 0f : -80f;

            _audioMixerGroup.audioMixer.SetFloat("SFXVolume", value);
        }

        private void OnBackToGameButtonClicked()
        {
            PageManager.Last();
            _unpauseAction?.Invoke();
        }

        private void OnBackToMenuButtonClicked()
        {
            _backToMenuAction?.Invoke();
        }

        private void OnDestroy()
        {
            _musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
            _sfxToggle.onValueChanged.RemoveListener(OnSfxToggleChanged);
            _backToGameButton.onClick.RemoveListener(OnBackToGameButtonClicked);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
        }
    }
}