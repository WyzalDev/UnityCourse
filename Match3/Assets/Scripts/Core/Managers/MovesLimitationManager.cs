// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using TMPro;
using UnityEngine;

namespace Core.Managers
{
    public class MovesLimitationManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private TMP_Text movesLimitationText;

        public Action OnMoveMade;

        private int _requiredMovesLimitation;
        private int _currentMovesLimitation;

        public bool IsMovesLimitationAchieved { get; private set; }

        public void Initialize(int movesLimitation)
        {
            _requiredMovesLimitation = movesLimitation;
            _currentMovesLimitation = 0;

            movesLimitationText.text = $"{_currentMovesLimitation} / {_requiredMovesLimitation}";
            IsMovesLimitationAchieved = false;
        }

        private void Start()
        {
            OnMoveMade += MoveMade;
        }

        private void MoveMade()
        {
            if (IsMovesLimitationAchieved)
                return;

            _currentMovesLimitation++;
            movesLimitationText.text = $"{_currentMovesLimitation} / {_requiredMovesLimitation}";
            IsMovesLimitationAchieved = _currentMovesLimitation >= _requiredMovesLimitation;
        }

        private void OnDestroy()
        {
            OnMoveMade -= MoveMade;
        }

        public void Restore()
        {
            _currentMovesLimitation = 0;
            movesLimitationText.text = $"{_currentMovesLimitation} / {_requiredMovesLimitation}";
            IsMovesLimitationAchieved = false;
        }
    }
}