using System;
using UnityEngine;

namespace Abilities
{
    public class AbilityCaster : MonoBehaviour, IHidden, IStageSettable, ICaster
    {
        private const float GravityDivider = 2f;
        
        private Transform _transform;
        private LineRenderer _line;
        private int _pointsCount;
        private int _stepBetweenPoints;
        private float _castStrength;

        public bool IsHidden { get; private set; }
        public Vector3 Position => _transform.position;
        public SatietyStage Stage { get; private set; }

        public event Action Hid;
        public event Action Showed;
        public event Action SpitCasted;

        public void Initialize(Transform transform, SatietyStage stage)
        {
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
            _line.positionCount = Mathf.CeilToInt(_pointsCount / _stepBetweenPoints);

            Vector3 start = _transform.position;
            Vector3 startVelocity = _castStrength * _transform.forward;
            int id = 0;
            
            _line.SetPosition(id, start);

            for (int time = 0; time < _pointsCount; time++)
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
            SpitCasted?.Invoke();
        }
    }
}