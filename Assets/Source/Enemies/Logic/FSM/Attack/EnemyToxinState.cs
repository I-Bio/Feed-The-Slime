using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemyToxinState : EnemyAttackState
    {
        private readonly ParticleSystem Particle;

        public EnemyToxinState(
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset,
            AudioSource sound,
            IPlayerVisitor visitor,
            ParticleSystem particle)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset, sound, visitor)
        {
            Particle = particle;
        }

        public override void Enter()
        {
            Particle.Play();
            StartAttack();
            OnInteract();
        }

        public override void Exit()
        {
            StopAttack();
            Particle.Stop();
        }

        public override void OnInteract()
        {
            Attack();
        }
    }
}