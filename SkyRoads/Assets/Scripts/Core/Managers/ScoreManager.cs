// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.MovingContainer;
using Core.Data;

namespace Core.Managers
{
    public class ScoreManager : MonoBehaviour, IBoostable, IRestorable
    {
        [SerializeField] private PlayerArea _playerArea;

        [Header("Score Settings")]
        [SerializeField] private int _asteroidPassedScorePoints;
        [SerializeField] private int _scorePointsForTime;
        [SerializeField] private int _boostedScorePointsForTime;
        [Tooltip("In seconds")]
        [SerializeField] private float _scoringPeriodTime;

        public long Score { get; private set; }
        public Action<long> OnScoreChanged;

        private int _currentTimeScore;
        private float _scoringCounts;

        private void Start()
        {
            _playerArea.AsteroidPassed += AsteroidPassed;

            _currentTimeScore = _scorePointsForTime;
            Restore();
        }

        public void Restore()
        {
            Score = 0;
            _scoringCounts = 0;
        }

        public void OnBoostStart(BoostData startParameters)
        {
            _currentTimeScore = _boostedScorePointsForTime;
        }

        public void OnBoostEnd(BoostData endParameters)
        {
            _currentTimeScore = _scorePointsForTime;
        }

        private void Update()
        {
            if (Timer.Value % 1000 > _scoringCounts)
            {
                Scoring();
                _scoringCounts++;
            }
        }

        private void Scoring()
        {
            Score += _currentTimeScore;
            InvokeOnScoreChanged(Score);
        }

        private void AsteroidPassed()
        {
            Score += _asteroidPassedScorePoints;
            InvokeOnScoreChanged(Score);
        }

        private void InvokeOnScoreChanged(long score)
        {
            OnScoreChanged?.Invoke(score);
        }

        private void OnDestroy()
        {
            _playerArea.AsteroidPassed -= AsteroidPassed;
        }
    }
}