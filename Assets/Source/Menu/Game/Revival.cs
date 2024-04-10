using UnityEngine;

namespace Menu
{
    public class Revival
    {
        private readonly Transform _player;
        private readonly Vector3 _startPosition;
        private readonly int _maxLifeCount;

        private int _currentLifeCount;
        
        public Revival(Transform player, int maxLifeCount)
        {
            _player = player;
            _maxLifeCount = maxLifeCount;
        }

        public bool TryRevive()
        {
            if (_currentLifeCount >= _maxLifeCount)
                return false;

            Revive();
            return true;
        }

        public void Revive()
        {
            _currentLifeCount++;
            _player.position = _startPosition;
        }
    }
}