using System.Collections.Generic;

namespace Enemies
{
    public class FiniteStateMachine
    {
        private Dictionary<EnemyStates, FiniteStateMachineState> _states;
        private FiniteStateMachineState _current;

        public void AddStates(Dictionary<EnemyStates, FiniteStateMachineState> states)
        {
            _states = states;
        }
        
        public void SetState(EnemyStates stateName)
        {
            if (_states.TryGetValue(stateName, out FiniteStateMachineState state) == false)
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