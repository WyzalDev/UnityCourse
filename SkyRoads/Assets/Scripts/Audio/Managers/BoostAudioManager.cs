// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.Audio;
using Core;
using Core.Data;

namespace Audio.Managers
{
    public class BoostAudioManager : MonoBehaviour, IBoostable, IPausable, IRestorable
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        private bool _isPaused;
        private bool _isBoosted;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (value)
                    _audioMixerGroup.audioMixer.SetFloat("BoostEngineVolume", -80f);
                else if (_isBoosted)
                    _audioMixerGroup.audioMixer.SetFloat("BoostEngineVolume", 0f);

                _isPaused = value;
            }
        }

        public void OnBoostStart(BoostData startParameters)
        {
            if (IsPaused)
                return;

            AudioManager.PlaySfxWithPitch("BoostOnSound");
            _audioMixerGroup.audioMixer.SetFloat("BoostEngineVolume", 0f);
            _isBoosted = true;
        }

        public void OnBoostEnd(BoostData endParameters)
        {
            if (IsPaused)
                return;

            AudioManager.PlaySfxWithPitch("BoostOffSound");
            _audioMixerGroup.audioMixer.SetFloat("BoostEngineVolume", -80f);
            _isBoosted = false;
        }

        public void Restore()
        {
            _audioMixerGroup.audioMixer.SetFloat("BoostEngineVolume", -80f);
            _isBoosted = false;
        }
    }
}