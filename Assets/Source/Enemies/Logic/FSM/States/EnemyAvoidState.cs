using UnityEngine;

namespace Enemies
{
    public abstract class EnemyAvoidState : EnemyState
    {
        public EnemyAvoidState(FinalStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset) {}

        public override void Update()
        {
            Destination = Player.IsHidden == false && Vector3.Distance(Transform.position, Player.Position) <= FollowDistance
                ? Transform.position - (Player.Position - Transform.position)
                : StartPosition;

            OnAvoid();
            
            if (Destination == StartPosition && Vector3.Distance(Transform.position, StartPosition) < IdleOffset)
                Machine.SetState(EnemyStates.Idle);
        }
        
        public virtual void OnAvoid() {}
    }
}