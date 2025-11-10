// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;

namespace Core.StateMachine
{
    public class FiniteStateMachine
    {
        private State _currentState;
        private readonly List<State> _states;

        public string Name { get; private set; }

        public FiniteStateMachineContext Context { get; private set; }

        public FiniteStateMachine(string name, FiniteStateMachineContext context)
        {
            Name = name;
            Context = context;
            _states = new List<State>();
        }

        public void AddState<T>(object initializeData) where T : State, new()
        {
            if (_states.Find(s => s.GetType() == typeof(T)) != null)
                return;

            var state = new T();

            state.Initialize(this, initializeData);
            _states.Add(state);
        }

        public void SetState<T>()
        {
            if (_currentState != null && _currentState.GetType() == typeof(T))
                return;

            var newState = _states.Find(s => s.GetType() == typeof(T));

            if (newState == null)
                return;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}