﻿using System.Collections.Generic;

namespace Enemies
{
    public class FinalStateMachine
    {
        private Dictionary<EnemyStates, FinalStateMachineState> States;
        private FinalStateMachineState _current;

        public void AddStates(Dictionary<EnemyStates, FinalStateMachineState> states)
        {
            States = states;
        }
        
        public void SetState(EnemyStates stateName)
        {
            if (States.TryGetValue(stateName, out FinalStateMachineState state) == false)
                return;
            
            _current?.Exit();
            _current = state;
            _current.Enter();
        }

        public void Update()
        {
            _current?.Update();
        }

        public void Exit()
        {
            _current?.Exit();
        }
    }
}