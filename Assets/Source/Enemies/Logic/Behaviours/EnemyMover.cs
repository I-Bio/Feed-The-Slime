using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMover : EnemyBehaviour
    {
        private NavMeshAgent _agent;
        private Vector3 _currentDestination;

        public new void Initialize(float delay)
        {
            _agent = GetComponent<NavMeshAgent>();
            base.Initialize(delay);
        }

        public override void Accept(IEnemyVisitor visitor, float thinkDelay)
        {
            visitor.Visit(this, thinkDelay);
        }

        public override void InteractInClose(Vector3 position) => Move(position);
        
        public override void AvoidInteraction(Vector3 position, Action onAvoided)
        {
            onAvoided?.Invoke();
            Move(position);
        }
        
        public override void CancelInteraction(){}

        private void Move(Vector3 position)
        {
            if (_currentDestination == position)
                return;

            _currentDestination = position;
            _agent.SetDestination(position);
        }
    }
}