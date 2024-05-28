using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Swarm : MonoBehaviour
    {
        private NavMeshAgent _agent;
        
        public void Initialize(float delay)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Move(Vector3 position)
        {
            _agent.SetDestination(position);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}