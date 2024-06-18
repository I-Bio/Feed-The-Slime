using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyEscapeState : EnemyAvoidState
    {
        private readonly NavMeshAgent Agent;

        public EnemyEscapeState(FiniteStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset,
            NavMeshAgent agent) :
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Agent = agent;
        }
        
        public override void Enter()
        {
            Destination = Transform.position - (Player.Position - Transform.position);
            Animation.Play(EnemyAnimations.Interact);
            OnAvoid();
        }

        public override void OnAvoid()
        {
            Agent.SetDestination(Destination);
        }
    }
}