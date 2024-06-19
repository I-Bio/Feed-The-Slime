using UnityEngine;

namespace Enemies
{
    public abstract class EnemyAvoidState : EnemyState
    {
        public EnemyAvoidState(
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
        }

        public override void Update()
        {
            SetDestination(CanInteract() == true ? CurrentPosition - (PlayerPosition - CurrentPosition) : StartPosition);

            OnAvoid();

            if (DidReturn() == true)
                SetState(EnemyStates.Idle);
        }

        public virtual void OnAvoid()
        {
        }
    }
}