using System;
using UnityEngine;

namespace Enemies
{
    public class Enemy
    {
        private readonly Transform _transform;
        private readonly IHidden _player;
        private readonly IEnemyPolicy _policy;
        private readonly SatietyStage _stage;
        private readonly Vector3 _startPosition;
        private readonly float _followDistance;

        private bool _isIdled;
        
        public Enemy(Transform transform, IHidden player, IEnemyPolicy policy, Vector3 startPosition, float followDistance)
        {
            _transform = transform;
            _player = player;
            _policy = policy;
            _startPosition = startPosition;
            _followDistance = followDistance;
        }

        public event Action<Vector3> Moved;
        public event Action Idled;

        public void CompareDistance()
        {
            if (_policy.CanMove(_player.IsHidden) == false)
                return;
            
            Vector3 currentPosition = _transform.position;
            float distance = Vector3.Distance(currentPosition, _player.Position);
            
            if (distance <= _followDistance)
            {
                if (_player.Stage < _stage)
                    Moved?.Invoke(_player.Position);
                else
                    Moved?.Invoke(-_player.Position.normalized * distance);
                
                _isIdled = false;
            }
            
            if (_isIdled == true)
                return;
            
            if (currentPosition == _startPosition)
            {
                _isIdled = true;
                Idled?.Invoke();
                return;
            }
            
            Moved?.Invoke(_startPosition);
        }
    }
}
