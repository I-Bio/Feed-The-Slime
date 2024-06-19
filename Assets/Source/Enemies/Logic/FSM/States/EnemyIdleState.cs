using UnityEngine;

namespace Enemies
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(
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

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Idle);
        }

        public override void Update()
        {
            if (CanInteract() == false)
                return;

            SetState(IsSmallerStage() == false ? EnemyStates.Interact : EnemyStates.Avoid);
        }
    }
}