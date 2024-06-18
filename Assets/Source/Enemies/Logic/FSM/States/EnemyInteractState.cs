using UnityEngine;

namespace Enemies
{
    public abstract class EnemyInteractState : EnemyState
    {
        public EnemyInteractState(FiniteStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset) {}
        
        public override void Update()
        {
            Destination = Player.IsHidden == false && Vector3.Distance(Transform.position, Player.Position) <= FollowDistance
                ? Player.Position
                : StartPosition;
            
            if (Player.Stage >= Stage)
            {
                Machine.SetState(EnemyStates.Avoid);
                return;
            }

            if (Destination == StartPosition && Vector3.Distance(Transform.position, StartPosition) < IdleOffset)
            {
                Machine.SetState(EnemyStates.Idle);
                return;
            }

            OnInteract();
        }
        
        public virtual void OnInteract() {}
    }
}