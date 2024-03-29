using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMover : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Vector3 _currentDestination;
        private float _delay;

        public event Action GoingMove;

        public void Initialize(float delay)
        {
            _agent = GetComponent<NavMeshAgent>();
            _delay = delay;
            StartCoroutine(MoveRoutine());
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