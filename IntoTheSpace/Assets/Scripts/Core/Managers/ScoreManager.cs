// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Data;
using UnityEngine;
using TMPro;

namespace Core.Managers
{
    public class ScoreManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private TMP_Text _scoreText;

        private void Start()
        {
            Score.Restore();
            Score.OnValueChanged += OnScoreChanged;
            _scoreText.text = "0";
        }

        private void OnScoreChanged()
        {
            _scoreText.text = Score.Value.ToString();
        }

        public void Restore()
        {
            Score.Restore();
        }

        private void OnDestroy()
        {
            Score.OnValueChanged -= OnScoreChanged;
        }
    }
}