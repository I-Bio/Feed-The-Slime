using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemySwarmState : EnemyAttackState
    {
        private readonly Swarm Swarm;
        
        public EnemySwarmState(FinalStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset,
            AudioSource sound, IPlayerVisitor visitor, Swarm swarm) :
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset, sound, visitor)
        {
            Swarm = swarm;
        }
        
        public override void Enter()
        {
            Swarm.Show();
            Sound.Play();
            Animation.Play(EnemyAnimations.Interact);
            OnInteract();
        }

        public override void Exit()
        {
            Visitor.Visit(null as EnemyEmpty);
            Swarm.Hide();
            Sound.Stop();
        }
        
        public override void OnInteract()
        {
            Swarm.Move(Player.Position);
            Visitor.Visit(this);
        }
    }
}