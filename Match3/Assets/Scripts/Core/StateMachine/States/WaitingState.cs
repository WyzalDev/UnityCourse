// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Data;
using Core.Managers;
using Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.StateMachine.States
{
    public class WaitingState : State
    {
        private GridManager _gridManager;
        private ChainManager _chainManager;
        private GoalsManager _goalsManager;
        private MovesLimitationManager _movesLimitationManager;
        private InputAction _interactAction;
        private InputAction _cursorPositionAction;
        private Camera _camera;

        public override void Initialize(FiniteStateMachine finiteStateMachine, object initializeData)
        {
            base.Initialize(finiteStateMachine, initializeData);

            _gridManager = finiteStateMachine.Context.GridManager;
            _chainManager = finiteStateMachine.Context.ChainManager;
            _goalsManager = finiteStateMachine.Context.GoalsManager;
            _movesLimitationManager = finiteStateMachine.Context.MovesLimitationManager;
            _interactAction = InputSystem.actions.FindAction("Interact");
            _cursorPositionAction = InputSystem.actions.FindAction("CursorPosition");
            _camera = Camera.main;
        }

        public override void Enter()
        {
            base.Enter();
            _interactAction.performed += HandleInteractAction;

            if (_movesLimitationManager.IsMovesLimitationAchieved || _goalsManager.IsGoalAchieved)
                FiniteStateMachine.SetState<EndGameState>();
        }

        private void HandleInteractAction(InputAction.CallbackContext ctx)
        {
            var cursorPosition = _cursorPositionAction.ReadValue<Vector2>();
            var worldCursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            var modelCoordinates = ViewModelCoordinatesConverter.GetModelCoordinates(worldCursorPosition);
            var element = _gridManager.GetElement(modelCoordinates.x, modelCoordinates.y);

            if (element == null)
                return;

            SelectedElement.SetNewSelectedElement(element);

            switch (element.Type)
            {
                case ElementType.RedGem:
                case ElementType.GreenGem:
                case ElementType.BlueGem:
                case ElementType.YellowGem:
                case ElementType.PurpleGem:
                    GemSelected();
                    break;
                case ElementType.Rocket:
                case ElementType.Bomb:
                    BonusSelected();
                    break;
            }
        }

        private void GemSelected()
        {
            if (!_chainManager.TryStartChain())
                return;

            FiniteStateMachine.SetState<InChainState>();
        }

        private void BonusSelected()
        {
            FiniteStateMachine.SetState<SelectedBonusActivationState>();
        }

        public override void Exit()
        {
            base.Exit();
            _interactAction.performed -= HandleInteractAction;
        }
    }
}