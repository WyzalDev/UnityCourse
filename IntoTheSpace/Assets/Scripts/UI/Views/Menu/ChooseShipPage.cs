// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using Core;
using Core.Data;
using UI.Data;
using UI.Managers;

namespace UI.Views.Menu
{
    public class ChooseShipPage : Page
    {
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Text _descriptionShipText;
        [SerializeField] private List<CharacterConfig> _characterConfigs;

        [Tooltip("Buttons, Images, Fade Images lists elements accordingly compared to Character Configs list elements")]
        [SerializeField] private List<Button> _shipButtons;

        [Tooltip("Buttons, Images, Fade Images lists elements accordingly compared to Character Configs list elements")]
        [SerializeField] private List<Image> _shipImages;

        [Tooltip("Buttons, Images, Fade Images lists elements accordingly compared to Character Configs list elements")]
        [SerializeField] private List<Image> _shipFadeImages;

        private int _selectedShipIndex;
        private List<UnityAction> _onClickActions = new List<UnityAction>();

        private void Start()
        {
            _acceptButton.onClick.AddListener(OnAcceptButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);

            if (_characterConfigs == null || _shipButtons == null || _shipImages == null || _shipFadeImages == null ||
                _characterConfigs.Count != _shipButtons.Count || _characterConfigs.Count != _shipImages.Count ||
                _characterConfigs.Count != _shipFadeImages.Count)
            {
#if UNITY_EDITOR
                Debug.LogError("CharacterConfigs, ShipButtons, ShipImages, ShipFadeImages elements count not equal");
#endif
                return;
            }

            for (var i = 0; i < _characterConfigs.Count; i++)
            {
                var index = i;
                UnityAction onClickAction = () => OnShipButtonClicked(index);

                _onClickActions.Add(onClickAction);
                _shipButtons[i].onClick.AddListener(onClickAction);
                SetShipAsIcon(_characterConfigs[index].ShipPrefab, _shipImages[index]);
            }

            OnShipButtonClicked(0);
        }

        private void SetShipAsIcon(Ship ship, Image image)
        {
            if (ship.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                image.sprite = spriteRenderer.sprite;
        }

        private void OnAcceptButtonClicked()
        {
            SceneManager.LoadScene("Game");
        }

        private void OnShipButtonClicked(int index)
        {
            _shipFadeImages[_selectedShipIndex].gameObject.SetActive(true);
            _shipButtons[_selectedShipIndex].interactable = true;

            _shipFadeImages[index].gameObject.SetActive(false);
            _shipButtons[index].interactable = false;

            _descriptionShipText.text = _characterConfigs[index].Description;
            GameStartData.ChosenShip = _characterConfigs[index].ShipPrefab;
            _selectedShipIndex = index;
        }

        private void OnBackButtonClicked()
        {
            PageManager.Last();
        }

        private void OnDestroy()
        {
            _acceptButton.onClick.RemoveListener(OnAcceptButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);

            for (var i = 0; i < _characterConfigs.Count; i++)
                _shipButtons[i].onClick.RemoveListener(_onClickActions[i]);
        }
    }
}