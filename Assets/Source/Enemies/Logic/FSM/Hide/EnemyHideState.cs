using UnityEngine;

namespace Enemies.Hide
{
    public class EnemyHideState : EnemyState
    {
        private readonly ParticleSystem Particle;
        
        public EnemyHideState(FiniteStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition,
            float idleOffset, ParticleSystem particle) :
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Particle = particle;
        }
        
        public override void Enter()
        {
            Animation.Play(EnemyAnimations.Idle);
            Particle.Play();
        }
        
        public override void Update()
        {
            if (Player.IsHidden == true || Vector3.Distance(Transform.position, Player.Position) > FollowDistance)
                return;

            Machine.SetState(EnemyStates.Action);
        }
    }
}