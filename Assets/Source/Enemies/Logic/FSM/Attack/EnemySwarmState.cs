using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemySwarmState : EnemyAttackState
    {
        private readonly Swarm Swarm;

        public EnemySwarmState(
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
            Swarm swarm)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset, sound, visitor)
        {
            Swarm = swarm;
        }

        public override void Enter()
        {
            Swarm.Show();
            StartAttack();
            OnInteract();
        }

        public override void Exit()
        {
            StopAttack();
            Swarm.Hide();
        }

        public override void OnInteract()
        {
            Swarm.Move(PlayerPosition);
            Attack();
        }
    }
}