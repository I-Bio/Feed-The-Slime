using UnityEngine;

namespace Enemies
{
    public class EnemyState : FinalStateMachineState
    {
        protected readonly IHidden Player;
        protected readonly Transform Transform;
        protected readonly EnemyAnimation Animation;
        protected readonly SatietyStage Stage;
        protected readonly float FollowDistance;
        protected readonly Vector3 StartPosition;
        protected readonly float IdleOffset;
        
        protected Vector3 Destination;

        public EnemyState(FinalStateMachine machine, IHidden player, Transform transform, EnemyAnimation animation, SatietyStage stage,
            float followDistance, Vector3 startPosition, float idleOffset) : base(machine)
        {
            Player = player;
            Transform = transform;
            Animation = animation;
            Stage = stage;
            FollowDistance = followDistance;
            StartPosition = startPosition;
            IdleOffset = idleOffset;
        }
    }
}