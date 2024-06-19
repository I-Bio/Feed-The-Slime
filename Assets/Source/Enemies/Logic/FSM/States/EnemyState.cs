using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyState : FiniteStateMachineState
    {
        private readonly IHidden Player;
        private readonly Transform Transform;
        private readonly EnemyAnimation Animation;
        private readonly SatietyStage Stage;
        private readonly float FollowDistance;
        private readonly float IdleOffset;

        public EnemyState(
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset)
            : base(machine)
        {
            Player = player;
            Transform = transform;
            Animation = animation;
            Stage = stage;
            FollowDistance = followDistance;
            StartPosition = startPosition;
            IdleOffset = idleOffset;
        }

        public Vector3 Destination { get; private set; }

        public Vector3 StartPosition { get; }

        public Vector3 PlayerPosition => Player.Position;

        public Vector3 CurrentPosition => Transform.position;

        public bool CanInteract()
        {
            return Player.IsHidden == false && Vector3.Distance(Transform.position, Player.Position) <= FollowDistance;
        }

        public bool IsSmallerStage()
        {
            return Stage <= Player.Stage;
        }

        public bool DidReturn()
        {
            return Destination == StartPosition && Vector3.Distance(Transform.position, StartPosition) < IdleOffset;
        }

        public void SetDestination(Vector3 destination)
        {
            Destination = destination;
        }

        public void PlayAnimation(EnemyAnimations animation, Action onPlayedCallback = null)
        {
            Animation.Play(animation, onPlayedCallback);
        }
    }
}