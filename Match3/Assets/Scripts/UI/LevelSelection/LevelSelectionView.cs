// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Audio.Managers;

namespace UI.LevelSelection
{
    public class LevelSelectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _elementNumber;
        [SerializeField] private Image _selectedFade;
        [SerializeField] private Button _button;
        [SerializeField] private string _clickButtonSFXName;

        public Action<LevelSelectionView> Clicked;

        public int LevelIndex { get; private set; }

        public void Initialize(int level)
        {
            _button.onClick.AddListener(InvokeOnClick);
            _elementNumber.text = level.ToString();
            LevelIndex = level;
        }

        public void Unselect()
        {
            _selectedFade.gameObject.SetActive(false);
        }

        public void Select()
        {
            _selectedFade.gameObject.SetActive(true);
        }

        private void InvokeOnClick()
        {
            Clicked?.Invoke(this);
            AudioManager.PlaySfxWithPitch(_clickButtonSFXName);
        }

        public void Unload()
        {
            _button.onClick.RemoveListener(InvokeOnClick);
        }
    }
}