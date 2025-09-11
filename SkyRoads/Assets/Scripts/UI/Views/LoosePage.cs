// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Views
{
    public class LoosePage : Page
    {
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Button _restartButton;

        private Action _restartAction;

        private void Start()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        public override void Show(object data = null)
        {
            if (data is PageData pageData)
            {
                _score.text = pageData.Score.ToString();
                _restartAction = pageData.Action;
            }

            base.Show(data);
        }

        private void OnRestartButtonClicked()
        {
            PageManager.Last();
            _restartAction?.Invoke();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
    }
}