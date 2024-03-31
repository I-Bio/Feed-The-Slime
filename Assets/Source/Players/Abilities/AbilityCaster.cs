using System;
using Abilities;
using Spawners;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(LineRenderer))]
    public class AbilityCaster : ObjectPool, IHidden, ICaster
    {
        private const float GravityDivider = 2f;
        
        [SerializeField] private int _pointsCount;
        [SerializeField] private float _stepBetweenPoints;
        [SerializeField] private float _castStrength;
        [SerializeField] private Vector3 _castOffset;
        
        private Transform _transform;
        private LineRenderer _line;

        public bool IsHidden { get; private set; }
        public Vector3 Position => _transform.position;
        public SatietyStage Stage { get; private set; }

        public event Action Hid;
        public event Action Showed;
        public event Action SpitCasted;

        public void Initialize(Transform transform, SatietyStage stage)
        {
            _line = GetComponent<LineRenderer>();
            _transform = transform;
            Stage = stage;
            IsHidden = false;
        }

        public void SetStage(SatietyStage stage)
        {
            Stage = stage;
        }

        public void Hide()
        {
            IsHidden = true;
            Hid?.Invoke();
        }

        public void Show()
        {
            IsHidden = false;
            Showed?.Invoke();
        }

        public void DrawCastTrajectory()
        {
            _line.enabled = true;
            _line.positionCount = Mathf.CeilToInt(_pointsCount / _stepBetweenPoints) + 1;

            Vector3 start = _transform.position + _castOffset;
            Vector3 startVelocity = _castStrength * (_transform.forward + Vector3.up);
            int id = 0;
            
            _line.SetPosition(id, start);

            for (float time = 0; time < _pointsCount; time +=_stepBetweenPoints)
            {
                Vector3 position = start + time * startVelocity;
                position.y = start.y + startVelocity.y * time + (Physics.gravity.y / GravityDivider * time * time);
                id++;
                
                _line.SetPosition(id, position);
            }
        }

        public void CastSpit()
        {
            _line.enabled = false;
            Pull<Projectile>(_transform.position + _castOffset).Initialize(_castStrength * (_transform.forward + Vector3.up));
            SpitCasted?.Invoke();
        }
    }
}