using UnityEngine;

namespace Enemies
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(FiniteStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset) {}
        
        public override void Enter()
        {
            Animation.Play(EnemyAnimations.Idle);
        }

        public override void Update()
        {
            if (Player.IsHidden == true || Vector3.Distance(Transform.position, Player.Position) > FollowDistance)
                return;

            Machine.SetState(Player.Stage < Stage ? EnemyStates.Interact : EnemyStates.Avoid);
        }
    }
}