using UnityEngine;

namespace Enemies.Hide
{
    public class EnemyShowState : EnemyState
    {
        private readonly ParticleSystem Particle;
        
        public EnemyShowState(FiniteStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition,
            float idleOffset, ParticleSystem particle) :
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Particle = particle;
        }

        public override void Enter()
        {
            Animation.Play(EnemyAnimations.Action, OnShowed);
            Particle.Play();
        }

        private void OnShowed()
        {
            Machine.SetState(Player.Stage < Stage ? EnemyStates.Interact : EnemyStates.Avoid);
        }
    }
}