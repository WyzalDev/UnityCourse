// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Managers;

namespace UI.Views.Menu
{
    public class ComicPage : Page
    {
        [SerializeField] private TMP_Text _comicText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextSlideButton;
        [SerializeField] private Button _previousSlideButton;
        [SerializeField] private ScrollRect _comicScrollRect;
        [SerializeField] [TextArea(3, 10)] private List<string> _slideTexts;

        public static bool IsFirstTime { get; set; } = true;

        private int _currentSlideNumber;

        private void Start()
        {
            _nextSlideButton.onClick.AddListener(OnNextSlideButtonClicked);
            _previousSlideButton.onClick.AddListener(OnPreviousSlideButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnNextSlideButtonClicked()
        {
            _currentSlideNumber++;

            SetPage(_slideTexts[_currentSlideNumber], 1f,
                _currentSlideNumber != _slideTexts.Count - 1,
                _currentSlideNumber != 0);
        }

        private void OnPreviousSlideButtonClicked()
        {
            _currentSlideNumber--;

            SetPage(_slideTexts[_currentSlideNumber], 1f,
                _currentSlideNumber != _slideTexts.Count - 1,
                _currentSlideNumber != 0);
        }

        private void SetPage(string text, float verticalNormalizedPosition, bool isNextSlideInteractable,
            bool isPreviousSlideInteractable)
        {
            _comicText.text = text;
            _comicScrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
            _nextSlideButton.interactable = isNextSlideInteractable;
            _previousSlideButton.interactable = isPreviousSlideInteractable;
        }

        private void OnBackButtonClicked()
        {
            PageManager.Last();
        }

        public override void Show(object data = null)
        {
            _currentSlideNumber = 0;
            SetPage(_slideTexts[_currentSlideNumber], 1f,
                true,
                false);

            base.Show(data);
        }

        private void OnDestroy()
        {
            _nextSlideButton.onClick.RemoveListener(OnNextSlideButtonClicked);
            _previousSlideButton.onClick.RemoveListener(OnPreviousSlideButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}