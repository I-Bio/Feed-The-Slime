namespace Enemies
{
    public class FiniteStateMachineState
    {
        public readonly FiniteStateMachine Machine;

        public FiniteStateMachineState(FiniteStateMachine machine)
        {
            Machine = machine;
        }

        public virtual void Enter() {}

        public virtual void Exit() {}

        public virtual void Update() {}
    }
}