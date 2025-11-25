// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using Audio.Managers;

namespace Audio.Controllers
{
    public class ToggleAudioController : BaseAudioController
    {
        [SerializeField] private Toggle _toggle;

        private const string _toggleClickSFXName = "UITogglePressed";

        private void Start()
        {
            _toggle.onValueChanged.AddListener(PlayAudio);
        }

        protected override void PlayAudio()
        {
            AudioManager.PlaySfx(_toggleClickSFXName);
        }

        private void PlayAudio(bool value)
        {
            PlayAudio();
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(PlayAudio);
        }
    }
}