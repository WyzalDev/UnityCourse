// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio.Managers;

namespace Audio.PlayableAudios
{
    public class ButtonsClickPlayableAudio : BasePlayableAudio
    {
        [SerializeField] private List<Button> _buttons;

        private const string ClickButtonAudioName = "UIButtonPressed";

        private void Start()
        {
            foreach (var button in _buttons)
                button.onClick.AddListener(PlayAudio);
        }

        protected override void PlayAudio()
        {
            AudioManager.PlaySfx(ClickButtonAudioName);
        }

        private void OnDestroy()
        {
            foreach (var button in _buttons)
                button.onClick.RemoveListener(PlayAudio);
        }
    }
}