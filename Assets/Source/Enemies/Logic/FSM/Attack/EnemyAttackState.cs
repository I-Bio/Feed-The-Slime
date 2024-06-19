using Players;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyAttackState : EnemyInteractState
    {
        private readonly AudioSource Sound;
        private readonly IPlayerVisitor Visitor;

        public EnemyAttackState(
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset,
            AudioSource sound,
            IPlayerVisitor visitor)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Sound = sound;
            Visitor = visitor;
        }

        public void Attack()
        {
            Visitor.Visit(this);
        }

        public void StopAttack()
        {
            Visitor.Visit(null as EnemyEmpty);
            Sound.Stop();
        }

        public void StartAttack()
        {
            Sound.Play();
            PlayAnimation(EnemyAnimations.Interact);
        }
    }
}