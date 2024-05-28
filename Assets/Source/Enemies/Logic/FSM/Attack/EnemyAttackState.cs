using Players;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyAttackState : EnemyInteractState
    {
        protected readonly AudioSource Sound;
        protected readonly IPlayerVisitor Visitor;
        
        public EnemyAttackState(FinalStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset,
            AudioSource sound, IPlayerVisitor visitor) : 
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Sound = sound;
            Visitor = visitor;
        }
    }
}