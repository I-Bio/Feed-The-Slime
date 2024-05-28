using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemyToxinState : EnemyAttackState
    {
        private readonly ParticleSystem Particle;
        
        public EnemyToxinState(FinalStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset,
            AudioSource sound, IPlayerVisitor visitor, ParticleSystem particle) : 
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset, sound, visitor)
        {
            Particle = particle;
        }

        public override void Enter()
        {
            Particle.Play();
            Sound.Play();
            Animation.Play(EnemyAnimations.Interact);
            OnInteract();
        }

        public override void Exit()
        {
            Visitor.Visit(null as EnemyEmpty);
            Particle.Stop();
            Sound.Stop();
        }
        
        public override void OnInteract()
        {
            Visitor.Visit(this);
        }
    }
}