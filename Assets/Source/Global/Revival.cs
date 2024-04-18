using UnityEngine;

public class Revival : MonoBehaviour
    {
        private Transform _player;
        private Vector3 _startPosition;
        private int _maxLifeCount;

        private int _currentLifeCount;
        
        public void Initialize(Transform player, int maxLifeCount)
        {
            _player = player;
            _startPosition = _player.position;
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
