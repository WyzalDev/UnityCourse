// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.Audio;

namespace Core.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        private static SettingsManager _instance;

        public static bool IsMusicOn
        {
            get
            {
                _instance._audioMixerGroup.audioMixer.GetFloat("MusicVolume", out var musicVolume);
                return !Mathf.Approximately(musicVolume, -80f);
            }
        }

        public static bool IsSfxOn
        {
            get
            {
                _instance._audioMixerGroup.audioMixer.GetFloat("SFXVolume", out var sfxVolume);
                return !Mathf.Approximately(sfxVolume, -80f);
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }
}