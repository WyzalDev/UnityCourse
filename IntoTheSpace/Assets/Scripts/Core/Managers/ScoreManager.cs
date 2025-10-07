// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;

namespace Core.Managers
{
    public class ScoreManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private TMP_Text _scoreText;

        private static ScoreManager _instance;

        private long _score;

        public static long Score => _instance._score;

        private void Start()
        {
            _instance = this;
            _scoreText.text = "0";
        }

        public static void AddScore(long value)
        {
            if (value <= 0)
                return;

            _instance._score += value;
            _instance._scoreText.text = Score.ToString();
        }

        public void Restore()
        {
            _score = 0;
            _instance._scoreText.text = Score.ToString();
        }
    }
}