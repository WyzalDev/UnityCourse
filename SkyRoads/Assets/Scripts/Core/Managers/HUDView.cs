// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using TMPro;

namespace Core.Managers
{
    public class HUDView : MonoBehaviour, IPausable
    {
        [SerializeField] private ScoreManager _scoreManager;

        [Header("Text Game Objects")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timerText;

        public bool IsPaused { get; set; }

        private void Start()
        {
            _scoreManager.OnScoreChanged += UpdateScoreText;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            _timerText.text = TimeSpan.FromSeconds(Timer.Value).ToString("mm':'ss':'fff");
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