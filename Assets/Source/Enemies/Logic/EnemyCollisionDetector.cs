using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemyCollisionDetector : MonoBehaviour
    {
        private bool _canContact = true;

        public void DisallowContact()
        {
            _canContact = false;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (_canContact == false)
                return;

            if (collision.collider.TryGetComponent(out PlayerCollisionDetector player) == false)
                return;

            player.ContactEnemy();
        }
    }
}