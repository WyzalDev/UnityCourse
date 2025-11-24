// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Core.Data;
using Core.Grid;
using Core.StateMachine;
using Core.StateMachine.States;
using Core.Utils;
using UI.Data;
using UI.LevelSelection;
using UI.Managers;
using UI.Views.Game;

namespace Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private ChainManager _chainManager;
        [SerializeField] private ElementSelectionManager _selectionManager;
        [SerializeField] private GoalsManager _goalsManager;
        [SerializeField] private MovesLimitationManager _movesLimitationManager;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private ElementsConfig _elementsConfig;
        [SerializeField] private CoroutineHolder _coroutineHolder;
        [SerializeField] private Transform _gemAnimationsParent;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Button _pauseButton;

        [SerializeField] private List<GameObject> _controllables;

        private FiniteStateMachine _finiteStateMachine;
        private FiniteStateMachineContext _stateMachineContext;
        private LevelConfig _levelConfig;
        private WaitForSecondsRealtime _cachedEndGameDelayWait;
        private List<IPausable> _pausables;
        private List<IRestorable> _restorables;

        private void Start()
        {
            _cachedEndGameDelayWait = new WaitForSecondsRealtime(_gameConfig.EndGameDelay);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);

            _pausables = new List<IPausable>();
            _restorables = new List<IRestorable>();

            foreach (var controllabe in _controllables)
            {
                var pausables = controllabe.GetComponents<IPausable>();
                if (pausables is not null && pausables.Length > 0)
                    _pausables.AddRange(pausables);

                var restorables = controllabe.GetComponents<IRestorable>();
                if (restorables is not null && restorables.Length > 0)
                    _restorables.AddRange(restorables);
            }

            var gridView = InstantiateGridView(_gridParent);

            _levelConfig = LevelSelectionManager.GetSelectedLevelConfig();

            InitializeGridManager(gridView);

            _movesLimitationManager.Initialize(_levelConfig.MovesLimitation);
            _goalsManager.Initialize(_levelConfig.GoalsConfig,
                gridView.GetSpriteByType(_levelConfig.GoalsConfig.GoalElementType));

            InitializeFsm();
            _finiteStateMachine.SetState<WaitingState>();
        }

        private ElementsGridView InstantiateGridView(Transform gridParent)
        {
            var gridViewGameObject = new GameObject("GridView");

            gridViewGameObject.transform.SetParent(gridParent);

            var gridView = gridViewGameObject.AddComponent<ElementsGridView>();

            return gridView;
        }

        private void InitializeGridManager(ElementsGridView gridView)
        {
            var grid = new ElementsGrid();

            grid.Initialize(_levelConfig.FieldConfig, new ElementTypesGenerator(_levelConfig.Seed));
            gridView.Initialize(grid, _elementsConfig, _gemAnimationsParent);
            _gridManager.Initialize(_levelConfig.FieldConfig, grid, gridView, _elementsConfig.RocketChainSize,
                _elementsConfig.BombChainSize);
        }

        private void InitializeFsm()
        {
            _stateMachineContext = new FiniteStateMachineContext(_gridManager, _chainManager, _goalsManager,
                _movesLimitationManager, _selectionManager, _coroutineHolder);

            _finiteStateMachine = new FiniteStateMachine("InGameFSM", _stateMachineContext);

            _finiteStateMachine.AddState<WaitingState>(_pausables);
            _finiteStateMachine.AddState<InChainState>(null);
            _finiteStateMachine.AddState<FallingState>(null);
            _finiteStateMachine.AddState<SelectedBonusActivationState>(null);
            _finiteStateMachine.AddState<EndGameState>(new Action(EndGame));
        }

        private void RestartGame()
        {
            RestoreGame();
            _finiteStateMachine.SetState<WaitingState>();
            UnpauseGame();
        }
        
        private void OnPauseButtonClicked()
        {
            PageManager.Show<PausePage>(new GamePageData()
            {
                ButtonAction = UnpauseGame,
                BackToMenuAction = BackToMenu
            });

            PauseGame();
        }

        private void PauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = true;

            Time.timeScale = 0;

            _pauseButton.interactable = false;
        }

        private void UnpauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = false;

            Time.timeScale = 1f;

            _pauseButton.interactable = true;
        }

        private void BackToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }

        private void RestoreGame()
        {
            foreach (var restorable in _restorables)
                restorable.Restore();
        }

        private void EndGame()
        {
            PauseGame();
            
            StartCoroutine(ShowEndGameUI());
        }

        private IEnumerator ShowEndGameUI()
        {
            yield return _cachedEndGameDelayWait;

            var pageData = new GamePageData()
            {
                ButtonAction = RestartGame,
                BackToMenuAction = BackToMenu
            };

            if (_goalsManager.IsGoalAchieved)
                PageManager.Show<WinPage>(pageData);
            else
                PageManager.Show<LoosePage>(pageData);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
            _finiteStateMachine.Unload();
        }
    }
}