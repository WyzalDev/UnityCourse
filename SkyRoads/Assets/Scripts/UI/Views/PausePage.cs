// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Views
{
    public class PausePage : Page
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private Button _backToGameButton;

        [Header("Fade Settings")]
        [SerializeField] private Image _fade;
        [SerializeField] private float _maxFadeValue;
        [SerializeField] private float _fadeDuration;

        private Action _unpauseAction;

        private void Start()
        {
            _backToGameButton.onClick.AddListener(OnBackToGameButtonClicked);
        }

        public override void Show(object data = null)
        {
            if (data is Action unpauseAction)
                _unpauseAction = unpauseAction;
            
            _fade.DOFade(_maxFadeValue, _fadeDuration)
                .SetEase(Ease.InQuart)
                .OnComplete(() => base.Show(data));
        }

        public override void Hide()
        {
            base.Hide();

            _fade.color = new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f);
        }

        public void OnMusicToggle(bool isOn)
        {
            var value = isOn ? 0f : -80f;

            _audioMixerGroup.audioMixer.SetFloat("MusicVolume", value);
        }

        public void OnSoundEffectsToggle(bool isOn)
        {
            var value = isOn ? 0f : -80f;

            _audioMixerGroup.audioMixer.SetFloat("SFXVolume", value);
        }

        private void OnBackToGameButtonClicked()
        {
            PageManager.Last();
            _unpauseAction?.Invoke();
        }

        private void OnDestroy()
        {
            _backToGameButton.onClick.RemoveListener(OnBackToGameButtonClicked);
        }
    }
}