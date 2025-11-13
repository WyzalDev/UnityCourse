// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;

namespace Core.StateMachine.States
{
    public class EndGameState : State
    {
        private Action _endGameAction;

        public override void Initialize(FiniteStateMachine finiteStateMachine, object initializeData)
        {
            base.Initialize(finiteStateMachine, initializeData);

            if (initializeData is Action endGameAction)
                _endGameAction = endGameAction;
        }

        public override void Enter()
        {
            base.Enter();

            _endGameAction?.Invoke();
        }
    }
}