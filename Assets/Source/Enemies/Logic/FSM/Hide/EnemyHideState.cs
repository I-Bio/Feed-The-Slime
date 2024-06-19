using UnityEngine;

namespace Enemies.Hide
{
    public class EnemyHideState : EnemyState
    {
        private readonly ParticleSystem Particle;

        public EnemyHideState(
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset,
            ParticleSystem particle)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Particle = particle;
        }

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Idle);
            Particle.Play();
        }

        public override void Update()
        {
            if (CanInteract() == false)
                return;

            SetState(EnemyStates.Action);
        }
    }
}