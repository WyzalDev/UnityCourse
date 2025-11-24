// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UI.Managers;

namespace UI.Views.Menu
{
    public class SettingsPage : Page
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _sfxToggle;
        [SerializeField] private Button _backToMenuButton;

        private void Start()
        {
            _musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            _sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        private void OnBackToMenuButtonClicked()
        {
            PageManager.Last();
        }

        public override void Show(object data = null)
        {
            CorrectToggles();
            base.Show(data);
        }

        private void CorrectToggles()
        {
            _musicToggle.isOn = SettingsManager.IsMusicOn;
            _sfxToggle.isOn = SettingsManager.IsSfxOn;
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

        private void OnDestroy()
        {
            _musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
            _sfxToggle.onValueChanged.RemoveListener(OnSfxToggleChanged);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
        }
    }
}