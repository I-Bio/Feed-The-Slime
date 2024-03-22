using UnityEngine;

namespace Players
{
    public class SizeScaler : MonoBehaviour
    {
        private Transform _transform;
        private float _scaleFactor;
        private Vector3 _startScale;

        public void Initialize(Transform transform, float scaleFactor)
        {
            _transform = transform;
            _scaleFactor = scaleFactor;
            _startScale = _transform.localScale;
        }

        public void Scale(SatietyStage stage)
        {
            _transform.localScale = _startScale * (_scaleFactor * (int)stage);
        }
    }
}