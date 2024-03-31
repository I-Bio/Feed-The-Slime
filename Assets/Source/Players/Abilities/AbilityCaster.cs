using System;
using Spawners;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(LineRenderer))]
    public class AbilityCaster : ObjectPool, IHidden, ICaster
    {
        [SerializeField] private int _pointsCount;
        [SerializeField] private float _castStrength;
        [SerializeField] private Vector3 _castOffset;
        [SerializeField] private EatableSpawner _spawner;
        
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
            _line.positionCount = _pointsCount;
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

            Vector3 start = _transform.position + _castOffset;
            Vector3 startVelocity = _castStrength * _transform.forward;

            for (int id = 0; id < _pointsCount; id++)
            {
                _line.SetPosition(id, start + id * startVelocity);
            }
        }

        public void CastSpit()
        {
            _line.enabled = false;
            Pull<Projectile>(_transform.position + _castOffset).Initialize(_castStrength * _transform.forward, _spawner);
            SpitCasted?.Invoke();
        }
    }
}