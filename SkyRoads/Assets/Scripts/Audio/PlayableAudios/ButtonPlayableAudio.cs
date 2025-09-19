// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio.Managers;

namespace Audio.PlayableAudios
{
    public class ButtonsClickPlayableAudio : BasePlayableAudio
    {
        [SerializeField] private List<GameObject> _controllables;

        private const string ClickButtonAudioName = "UIButtonClickSound";

        private void Start()
        {
            foreach (var controllable in _controllables)
            {
                if (controllable.TryGetComponent<Button>(out var button))
                    button.onClick.AddListener(PlayAudio);

                if (controllable.TryGetComponent<Toggle>(out var toggle))
                    toggle.onValueChanged.AddListener(PlayAudio);
            }
        }

        protected override void PlayAudio()
        {
            AudioManager.PlaySfxWithPitch(ClickButtonAudioName);
        }

        private void PlayAudio(bool mock)
        {
            PlayAudio();
        }

        private void OnDestroy()
        {
            foreach (var controllable in _controllables)
            {
                if (controllable.TryGetComponent<Button>(out var button))
                    button.onClick.RemoveListener(PlayAudio);

                if (controllable.TryGetComponent<Toggle>(out var toggle))
                    toggle.onValueChanged.RemoveListener(PlayAudio);
            }
        }
    }
}