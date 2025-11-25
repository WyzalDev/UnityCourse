// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Data;
using Core.Grid;
using Core.Managers;

namespace Core.StateMachine.States
{
    public class SelectedBonusActivationState : State
    {
        private GridManager _gridManager;
        private ElementSelectionManager _selectionManager;
        private InputAction _interactAction;
        private InputAction _cursorPositionAction;
        private Camera _camera;

        private Element _selectedElement;

        public override void Initialize(FiniteStateMachine finiteStateMachine, object initializeData)
        {
            base.Initialize(finiteStateMachine, initializeData);

            _gridManager = finiteStateMachine.Context.GridManager;
            _selectionManager = finiteStateMachine.Context.SelectionManager;
            _interactAction = InputSystem.actions.FindAction("Interact");
        }

        public override void Enter()
        {
            base.Enter();
            _selectionManager.IsActive = true;
            _interactAction.canceled += ActivateBonus;

            _selectedElement = SelectedElement.Element;
        }

        private void ActivateBonus(InputAction.CallbackContext ctx)
        {
            if (SelectedElement.Element != _selectedElement)
                FiniteStateMachine.SetState<WaitingState>();

            _gridManager.MatchElements(new List<Element> { _selectedElement });
            _gridManager.DestroyGemsAnimation();
            FiniteStateMachine.SetState<FallingState>();
        }

        public override void Exit()
        {
            base.Exit();
            _selectionManager.IsActive = false;
            _interactAction.canceled -= ActivateBonus;
        }
    }
}