// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Core.Data;

namespace Core.Managers
{
    public class VignetteManager : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private VignetteConfig _takeDamageVignetteConfig;

        private static VignetteManager _instance;

        private Vignette _vignette;
        private Coroutine _animationCoroutine;
        private WaitForSeconds _cachedDelay;
        private float _appearanceTime;
        private float _disappearanceTime;

        private void Start()
        {
            _instance = this;

            _cachedDelay = new WaitForSeconds(_takeDamageVignetteConfig.ScreenTime);
            _appearanceTime = _takeDamageVignetteConfig.AppearanceTime;
            _disappearanceTime = _takeDamageVignetteConfig.DisappearanceTime;

            if (!_volume.profile.TryGet(out _vignette))
                Debug.LogError("Vignette not found");
        }

        public static void StartTakeDamageEffect()
        {
            if (_instance._animationCoroutine != null)
            {
                _instance.StopCoroutine(_instance.TakeDamageEffect());
                _instance._animationCoroutine = null;
            }

            _instance._animationCoroutine = _instance.StartCoroutine(_instance.TakeDamageEffect());
        }

        private IEnumerator TakeDamageEffect()
        {
            yield return DOVirtual.Float(_instance._vignette.intensity.value,
                _instance._takeDamageVignetteConfig.Intensity, _instance._appearanceTime,
                value => _instance._vignette.intensity.value = value).WaitForCompletion();

            yield return _cachedDelay;

            yield return DOVirtual.Float(_instance._vignette.intensity.value,
                0f, _disappearanceTime,
                value => _instance._vignette.intensity.value = value).WaitForCompletion();

            _instance._animationCoroutine = null;
        }
    }
}