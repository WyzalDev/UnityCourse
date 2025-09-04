// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;
using Core.MovingContainer;

namespace Core.Managers
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private PlayerArea _playerArea;
        [SerializeField] private ScoreManager _scoreManager;
        
        [Header("Text Game Objects")]
        [SerializeField] private TMP_Text _scoreText;

        private void Start()
        {
            _scoreManager.OnScoreChanged += UpdateScoreText;
        }

        private void UpdateScoreText(long score)
        {
            _scoreText.text = $"Score: {score}";
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= UpdateScoreText;
        }
    }
}