// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Data;
using Core.Managers;
using UnityEngine.InputSystem;

namespace Core.StateMachine.States
{
    public class InChainState : State
    {
        private GridManager _gridManager;
        private ChainManager _chainManager;
        private ElementSelectionManager _selectionManager;

        private InputAction _interactAction;

        public override void Initialize(FiniteStateMachine finiteStateMachine, object initializeData)
        {
            base.Initialize(finiteStateMachine, initializeData);

            _gridManager = finiteStateMachine.Context.GridManager;
            _chainManager = finiteStateMachine.Context.ChainManager;
            _selectionManager = finiteStateMachine.Context.SelectionManager;
            _interactAction = InputSystem.actions.FindAction("Interact");
        }

        public override void Enter()
        {
            base.Enter();
            _interactAction.canceled += EndChain;
            _selectionManager.OnSelectionEnd += TryChangeElementInChain;
            _selectionManager.IsActive = true;
        }

        private void TryChangeElementInChain()
        {
            _chainManager.TryChangeElementInChain(SelectedElement.Element);
        }

        private void EndChain(InputAction.CallbackContext ctx)
        {
            if (!_chainManager.EndChain())
            {
                _chainManager.Restore();
                FiniteStateMachine.SetState<WaitingState>();
                return;
            }

            _gridManager.MatchElements(_chainManager.ChainElements);
            _chainManager.Restore();
            _gridManager.DestroyGemsAnimation();
            FiniteStateMachine.SetState<FallingState>();
        }

        public override void Exit()
        {
            base.Exit();
            _interactAction.canceled -= EndChain;
            _selectionManager.OnSelectionEnd -= TryChangeElementInChain;
            _selectionManager.IsActive = false;
        }
    }
}