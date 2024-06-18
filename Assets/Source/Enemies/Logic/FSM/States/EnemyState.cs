using UnityEngine;

namespace Enemies
{
    public class EnemyState : FiniteStateMachineState
    {
        public readonly IHidden Player;
        public readonly Transform Transform;
        public readonly EnemyAnimation Animation;
        public readonly SatietyStage Stage;
        public readonly float FollowDistance;
        public readonly Vector3 StartPosition;
        public readonly float IdleOffset;
        
        public Vector3 Destination;

        public EnemyState(FiniteStateMachine machine, IHidden player, Transform transform, EnemyAnimation animation, SatietyStage stage,
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