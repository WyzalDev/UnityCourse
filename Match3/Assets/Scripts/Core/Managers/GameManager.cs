// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Data;
using Core.Grid;
using Core.StateMachine;
using Core.StateMachine.States;
using Core.Utils;

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
        [SerializeField] private LineRenderer _chainLineRenderer;
        [SerializeField] private CoroutineHolder _coroutineHolder;
        [SerializeField] private Transform _gemAnimationsParent;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _unpauseButton;
        [SerializeField] private Button _restartButton;

        [SerializeField] private List<GameObject> _controllables;

        private FiniteStateMachine _finiteStateMachine;
        private FiniteStateMachineContext _stateMachineContext;
        private LevelConfig _levelConfig;
        private WaitForSeconds _cachedEndGameDelayWait;
        private List<IPausable> _pausables;
        private List<IRestorable> _restorables;

        private const string LevelConfigPath = "Levels/TestLevel";

        private void Start()
        {
            _cachedEndGameDelayWait = new WaitForSeconds(_gameConfig.EndGameDelay);
            _pauseButton.onClick.AddListener(PauseGame);
            _unpauseButton.onClick.AddListener(UnpauseGame);
            _restartButton.onClick.AddListener(RestartGame);

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

            var loadedConfig = Resources.Load<TextAsset>(LevelConfigPath);
            var gridView = InstantiateGridView(_gridParent);

            _levelConfig = JsonUtility.FromJson<LevelConfig>(loadedConfig.text);

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

            _finiteStateMachine.AddState<WaitingState>(null);
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

            _restartButton.interactable = false;
        }

        private void PauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = true;

            Time.timeScale = 0;

            _pauseButton.interactable = false;
            _unpauseButton.interactable = true;
        }

        private void UnpauseGame()
        {
            foreach (var pausable in _pausables)
                pausable.IsPaused = false;

            Time.timeScale = 1f;

            _pauseButton.interactable = true;
            _unpauseButton.interactable = false;
        }

        private void RestoreGame()
        {
            foreach (var restorable in _restorables)
                restorable.Restore();
        }

        private void EndGame()
        {
            _restartButton.interactable = true;
            PauseGame();
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(PauseGame);
            _unpauseButton.onClick.RemoveListener(UnpauseGame);
            _restartButton.onClick.RemoveListener(RestartGame);
        }
    }
}