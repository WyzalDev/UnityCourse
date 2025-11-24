// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UI.LevelSelection;
using UI.Managers;

namespace UI.Views.Menu
{
    public class ChooseLevelPage : Page
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _backToMenuButton;

        [Header("Scroll View Settings")]
        [SerializeField] private Transform _scrollViewContent;
        [SerializeField] private LevelSelectionView _levelSelectionViewPrefab;

        private Dictionary<int, LevelSelectionView> _levelSelectionViews;
        private static LevelSelectionView _selectedLevelSelectionView;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("Game");
        }

        private void OnBackToMenuButtonClicked()
        {
            PageManager.Last();
        }

        public override void Show(object data = null)
        {
            base.Show(data);

            _playButton.interactable = _selectedLevelSelectionView is not null;
            _levelSelectionViews ??= new Dictionary<int, LevelSelectionView>();
            var levelsCount = LevelSelectionManager.LevelsCount;

            for (var i = 1; i <= levelsCount; i++)
            {
                var levelSelectionView = Instantiate(_levelSelectionViewPrefab, _scrollViewContent);
                levelSelectionView.Initialize(i);
                levelSelectionView.Unselect();
                levelSelectionView.Clicked += OnViewClick;

                _levelSelectionViews.Add(i, levelSelectionView);
            }
        }

        private void OnViewClick(LevelSelectionView levelSelectionView)
        {
            if (_selectedLevelSelectionView != levelSelectionView)
                SelectLevel(levelSelectionView);
            else
                DeselectLevel();
        }

        private void SelectLevel(LevelSelectionView levelSelectionView)
        {
            DeselectLevel();

            LevelSelectionManager.SelectNewLevel(levelSelectionView.LevelIndex);
            levelSelectionView.Select();
            _selectedLevelSelectionView = levelSelectionView;

            _playButton.interactable = true;
        }

        private void DeselectLevel()
        {
            if (_selectedLevelSelectionView == null)
                return;

            _selectedLevelSelectionView.Unselect();
            _selectedLevelSelectionView = null;

            _playButton.interactable = false;
        }

        public override void Hide()
        {
            base.Hide();

            DeselectLevel();

            if (_levelSelectionViews == null)
                return;

            foreach (var levelSelectionView in _levelSelectionViews.Values)
            {
                levelSelectionView.Clicked -= OnViewClick;
                levelSelectionView.Unload();
                Destroy(levelSelectionView.gameObject);
            }

            _levelSelectionViews.Clear();
        }

        private void OnDestroy()
        {
            Hide();
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClicked);
        }
    }
}