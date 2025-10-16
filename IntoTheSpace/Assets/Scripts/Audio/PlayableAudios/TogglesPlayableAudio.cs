// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio.Managers;

namespace Audio.PlayableAudios
{
    public class TogglesPlayableAudio : BasePlayableAudio
    {
        [SerializeField] private List<Toggle> _toggles;

        private const string ClickToggleAudioName = "UITogglePressed";

        private void Start()
        {
            foreach (var toggle in _toggles)
                toggle.onValueChanged.AddListener(PlayAudio);
        }

        protected override void PlayAudio()
        {
            AudioManager.PlaySfx(ClickToggleAudioName);
        }

        private void PlayAudio(bool mock)
        {
            PlayAudio();
        }

        private void OnDestroy()
        {
            foreach (var toggle in _toggles)
                toggle.onValueChanged.RemoveListener(PlayAudio);
        }
    }
}