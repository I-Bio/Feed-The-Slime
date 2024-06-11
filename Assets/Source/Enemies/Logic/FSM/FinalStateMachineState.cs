namespace Enemies
{
    public class FinalStateMachineState
    {
        public readonly FinalStateMachine Machine;

        public FinalStateMachineState(FinalStateMachine machine)
        {
            Machine = machine;
        }

        public virtual void Enter() {}

        public virtual void Exit() {}

        public virtual void Update() {}
    }
}