namespace Enemies
{
    public class FiniteStateMachineState
    {
        private readonly FiniteStateMachine Machine;

        public FiniteStateMachineState(FiniteStateMachine machine)
        {
            Machine = machine;
        }

        public void SetState(EnemyStates state)
        {
            Machine.SetState(state);
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }
    }
}