// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        [SerializeField] private AudioStorage _storage;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioSource _sfxAudioSourceWithPitch;
        [SerializeField] private AudioSource _sfxAudioSourceWithLoop;

        [Header("SFX Settings")]
        [SerializeField] private float _pitchMin;
        [SerializeField] private float _pitchMax;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else if (_instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            PlayLoopSFX("BoostEngine");
        }

        public static void PlayMusic(string musicName)
        {
            if (_instance == null)
                return;

            if (!_instance._storage.TryGetSoundByName(musicName, out var sound))
                return;

            _instance._musicAudioSource.Stop();
            _instance._musicAudioSource.clip = sound.Clip;
            _instance._musicAudioSource.Play();
        }

        private static void PlayLoopSFX(string sfxName)
        {
            if (_instance == null)
                return;

            if (!_instance._storage.TryGetSoundByName(sfxName, out var sound))
                return;

            _instance._sfxAudioSourceWithLoop.Stop();
            _instance._sfxAudioSourceWithLoop.clip = sound.Clip;
            _instance._sfxAudioSourceWithLoop.Play();
        }

        public static void PlaySfx(string sfxName)
        {
            if (_instance == null)
                return;

            if (!_instance._storage.TryGetSoundByName(sfxName, out var sound))
                return;

            _instance._sfxAudioSource.PlayOneShot(sound.Clip);
        }

        public static void PlaySfxWithPitch(string sfxName)
        {
            if (_instance == null)
                return;

            if (!_instance._storage.TryGetSoundByName(sfxName, out var sound))
                return;

            _instance._sfxAudioSourceWithPitch.pitch = Random.Range(_instance._pitchMin, _instance._pitchMax);
            _instance._sfxAudioSourceWithPitch.PlayOneShot(sound.Clip);
        }
    }
}