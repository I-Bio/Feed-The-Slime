using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemyCollisionDetector : MonoBehaviour
    {
        private SatietyStage _stage;

        public void Initialize(SatietyStage stage)
        {
            _stage = stage;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IPlayerVisitor player) == false)
                return;

            player.Visit(null, _stage);
        }
    }
}