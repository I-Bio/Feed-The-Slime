using System;
using Spawners;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(LineRenderer))]
    public class AbilityCaster : ObjectPool, IHidden, ICaster
    {
        private LineRenderer _line;
        private Transform _transform;
        private int _pointsCount;
        private float _castStrength;
        private Vector3 _castOffset;
        private EatableSpawner _spawner;
        private AbilityButton _spitButton;

        public bool IsHidden { get; private set; }
        public Vector3 Position => _transform.position;
        public SatietyStage Stage { get; private set; }

        public event Action Hid;
        public event Action Showed;
        public event Action SpitCasted;

        public void Initialize(Transform transform, SatietyStage stage, int pointsCount, float castStrength,
            Vector3 castOffset, EatableSpawner spawner, AbilityButton spitButton, Projectile projectile)
        {
            Initialize(projectile);
            _transform = transform;
            Stage = stage;
            _pointsCount = pointsCount;
            _castStrength = castStrength;
            _castOffset = castOffset;
            _spawner = spawner;
            _spitButton = spitButton;
            _spitButton.Initialize();
            _line = GetComponent<LineRenderer>();
            _line.positionCount = _pointsCount;
            _line.enabled = false;
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
            if (_spitButton.CanUse == false)
                return;

            _line.enabled = true;

            Vector3 start = _transform.position + _castOffset;
            Vector3 startVelocity = _castStrength * _transform.forward;

            for (int id = 0; id < _pointsCount; id++)
                _line.SetPosition(id, start + id * startVelocity);
        }

        public void CastSpit()
        {
            if (_spitButton.CanUse == false)
                return;

            _line.enabled = false;
            _spitButton.Use();
            Pull<Projectile>(_transform.position + _castOffset)
                .Initialize(_castStrength * _transform.forward, _spawner);
            SpitCasted?.Invoke();
        }
    }
}