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

        public Enemy(Transform transform, IHidden player, IEnemyPolicy policy, SatietyStage stage, float followDistance)
        {
            _transform = transform;
            _player = player;
            _policy = policy;
            _stage = stage;
            _startPosition = _transform.position;
            _followDistance = followDistance;
            _isIdled = true;
        }

        public event Action<Vector3> Moved;
        public event Action<Vector3> RunningAway;
        public event Action Idled;

        public void CompareDistance()
        {
            if (_policy.CanMove(_player.IsHidden) == true)
            {
                float distance = Vector3.Distance(_transform.position, _player.Position);

                if (distance <= _followDistance)
                {
                    if (_player.Stage < _stage)
                        Moved?.Invoke(_player.Position);
                    else
                        RunningAway?.Invoke(_transform.position - (_player.Position - _transform.position));

                    _isIdled = false;
                    return;
                }
            }

            if (_isIdled == true)
                return;

            if (_transform.position == _startPosition)
            {
                Idled?.Invoke();
                _isIdled = true;
                return;
            }

            Moved?.Invoke(_startPosition);
        }
    }
}