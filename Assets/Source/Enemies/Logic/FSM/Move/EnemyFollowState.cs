using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyFollowState : EnemyInteractState
    {
        private readonly NavMeshAgent Agent;
        
        public EnemyFollowState(FiniteStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance,
            Vector3 startPosition, float idleOffset, NavMeshAgent agent) :
            base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
            Agent = agent;
        }

        public override void Enter()
        {
            Destination = Player.Position;
            Animation.Play(EnemyAnimations.Interact);
            OnInteract();
        }

        public override void OnInteract()
        {
            Agent.SetDestination(Destination);
        }
    }
}