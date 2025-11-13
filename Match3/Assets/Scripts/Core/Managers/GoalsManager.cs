// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.Data;

namespace Core.Managers
{
    public class GoalsManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private Transform _elementGoalParent;
        [SerializeField] private Transform _scoreGoalParent;
        
        [Header("Element Goal")]
        [SerializeField] private Image _elementGoalImage;
        [SerializeField] private TMP_Text _goalText;

        [Header("Score Goal")]
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private TMP_Text _scoreText;

        private GoalType _goalType;
        private ElementType _elementType;
        private int _requiredValue;
        private int _currentValue;
        private int _scoreMultiplier;

        public bool IsGoalAchieved { get; private set; }

        public void Initialize(GoalsConfig config, Sprite elementSprite)
        {
            _goalType = config.GoalType;
            _elementType = config.GoalElementType;
            _requiredValue = config.RequiredValue;
            _currentValue = 0;

            switch (_goalType)
            {
                case GoalType.GetScore:
                    _scoreMultiplier = config.ScoreMultiplier;

                    _elementGoalParent.gameObject.SetActive(false);
                    _scoreGoalParent.gameObject.SetActive(true);

                    _scoreSlider.minValue = 0;
                    _scoreSlider.maxValue = _requiredValue;
                    _scoreSlider.value = _currentValue;
                    _scoreText.text = _currentValue.ToString();

                    break;
                case GoalType.DestroyGems or GoalType.DestroyObstacles:
                    _scoreMultiplier = 1;

                    _scoreGoalParent.gameObject.SetActive(false);
                    _elementGoalParent.gameObject.SetActive(true);

                    _elementGoalImage.sprite = elementSprite;
                    _goalText.text = $"{_currentValue} / {_requiredValue}";

                    break;
            }
        }

        public void TryAddGoalScore(ElementType elementType)
        {
            if (IsGoalAchieved)
                return;

            switch (_goalType)
            {
                case GoalType.DestroyObstacles or GoalType.DestroyGems when elementType == _elementType:
                {
                    _currentValue += _scoreMultiplier;
                    _goalText.text = $"{_currentValue} / {_requiredValue}";
                    IsGoalAchieved = _currentValue >= _requiredValue;

                    break;
                }
                case GoalType.GetScore when elementType.IsGem():
                    _currentValue += _scoreMultiplier;
                    _scoreSlider.value = _currentValue;
                    _scoreText.text = _currentValue.ToString();
                    IsGoalAchieved = _currentValue >= _requiredValue;

                    break;
            }
        }

        public void Restore()
        {
            _currentValue = 0;
            if (_goalType is GoalType.GetScore)
                _scoreSlider.value = _currentValue;

            _goalText.text = $"{_currentValue} / {_requiredValue}";
            _scoreText.text = _currentValue.ToString();
            IsGoalAchieved = false;
        }
    }
}