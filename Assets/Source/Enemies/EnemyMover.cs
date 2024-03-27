using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private float _delay;
        
        private NavMeshAgent _agent;
        private Vector3 _currentDestination;

        public event Action GoingMove;
        
        public void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Move(Vector3 position)
        {
            if (_currentDestination == position)
                return;

            _currentDestination = position;
            _agent.SetDestination(position);
        }

        private IEnumerator MoveRoutine()
        {
            bool isWorking = true;
            var wait = new WaitForSeconds(_delay);

            while (isWorking)
            {
                GoingMove?.Invoke();
                yield return wait;
            }
        }
    }
}