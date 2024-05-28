using UnityEngine;

namespace Enemies
{
    public abstract class EnemyInteractState : EnemyState
    {
        public EnemyInteractState(FinalStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset) :
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset) {}
        
        public override void Update()
        {
            Destination = Player.IsHidden == false && Vector3.Distance(Transform.position, Player.Position) <= FollowDistance
                ? Player.Position
                : StartPosition;

            OnInteract();

            if (Player.Stage >= Stage)
            {
                Machine.SetState(EnemyStates.Avoid);
                return;
            }

            if (Destination == StartPosition && Vector3.Distance(Transform.position, StartPosition) < IdleOffset)
                Machine.SetState(EnemyStates.Idle);
        }
        
        public virtual void OnInteract() {}
    }
}