// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Managers;

namespace Core.StateMachine
{
    public class FiniteStateMachineContext
    {
        public GridManager GridManager { get; private set; }

        public GoalsManager GoalsManager { get; private set; }

        public MovesLimitationManager MovesLimitationManager { get; private set; }

        public ChainManager ChainManager { get; private set; }

        public ElementSelectionManager SelectionManager { get; private set; }

        public CoroutineHolder CoroutineHolder { get; private set; }

        public FiniteStateMachineContext(GridManager gridManager, ChainManager chainManager, GoalsManager goalsManager,
            MovesLimitationManager movesLimitationManager, ElementSelectionManager selectionManager,
            CoroutineHolder coroutineHolder)
        {
            GridManager = gridManager;
            GoalsManager = goalsManager;
            MovesLimitationManager = movesLimitationManager;
            ChainManager = chainManager;
            SelectionManager = selectionManager;
            CoroutineHolder = coroutineHolder;
        }
    }
}