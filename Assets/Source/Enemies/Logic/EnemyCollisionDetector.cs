using Players;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyCollisionDetector : Contactable
    {
        private SatietyStage _stage;
        private IPlayerVisitor _player;
        private Collider _collider;

        public void Initialize(SatietyStage stage, IPlayerVisitor player)
        {
            _stage = stage;
            _player = player;
            _collider = GetComponent<Collider>();
        }

        public override bool TryContact(Bounds bounds)
        {
            if (bounds.Intersects(_collider.bounds) == false)
                return false;
            
            _player.Visit(null, _stage);
            return true;
        }
    }
}