using UnityEngine;

namespace Enemies
{
    public abstract class EnemyInteractState : EnemyState
    {
        public EnemyInteractState(
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
            SetDestination(CanInteract() == true ? PlayerPosition : StartPosition);

            if (IsSmallerStage() == true)
            {
                SetState(EnemyStates.Avoid);
                return;
            }

            if (DidReturn() == true)
            {
                SetState(EnemyStates.Idle);
                return;
            }

            OnInteract();
        }

        public virtual void OnInteract()
        {
        }
    }
}