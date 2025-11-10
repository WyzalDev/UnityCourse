// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections;
using Core.Managers;

namespace Core.StateMachine.States
{
    public class FallingState : State
    {
        private GridManager _gridManager;
        private CoroutineHolder _coroutineHolder;

        public override void Initialize(FiniteStateMachine finiteStateMachine, object initializeData)
        {
            base.Initialize(finiteStateMachine, initializeData);

            _gridManager = finiteStateMachine.Context.GridManager;
            _coroutineHolder = finiteStateMachine.Context.CoroutineHolder;
        }

        public override void Enter()
        {
            base.Enter();

            _coroutineHolder.StartCoroutine(OnEnterState());
        }

        private IEnumerator OnEnterState()
        {
            yield return _gridManager.WaitOnDestroyGemsAnimations();

            var elementsFallData = _gridManager.ApplyFallingAndSpawning();

            yield return _gridManager.ElementsFallingAnimation(elementsFallData);

            FiniteStateMachine.SetState<WaitingState>();
        }
    }
}