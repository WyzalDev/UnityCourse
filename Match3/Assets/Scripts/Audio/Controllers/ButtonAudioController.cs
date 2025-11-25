// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using Audio.Managers;

namespace Audio.Controllers
{
    public class ButtonAudioController : BaseAudioController
    {
        [SerializeField] private Button _button;

        private const string _buttonClickSFXName = "UIButtonPressed";

        private void Start()
        {
            _button.onClick.AddListener(PlayAudio);
        }

        protected override void PlayAudio()
        {
            AudioManager.PlaySfx(_buttonClickSFXName);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PlayAudio);
        }
    }
}