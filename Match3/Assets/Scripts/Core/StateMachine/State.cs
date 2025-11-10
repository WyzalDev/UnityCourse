// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.StateMachine
{
    public class State
    {
        protected FiniteStateMachine FiniteStateMachine;

        public virtual void Initialize(FiniteStateMachine finiteStateMachine, object initializeData)
        {
            FiniteStateMachine = finiteStateMachine;
        }

        public virtual void Enter()
        {
#if UNITY_EDITOR
            Debug.Log($"{FiniteStateMachine.Name}: Enter [{GetType().Name}] state");
#endif
        }

        public virtual void Exit()
        {
#if UNITY_EDITOR
            Debug.Log($"{FiniteStateMachine.Name}: Exit [{GetType().Name}] state -> switching states");
#endif
        }
    }
}