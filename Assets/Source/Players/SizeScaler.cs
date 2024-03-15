using UnityEngine;

namespace Players
{
    public class SizeScaler : MonoBehaviour
    {
        private Transform _transform;
        private float _scaleFactor;

        public void Initialize(Transform transform, float scaleFactor)
        {
            _transform = transform;
            _scaleFactor = scaleFactor;
        }

        public void Scale(SatietyStage stage)
        {
            _transform.localScale *= _scaleFactor * (int)stage;
        }
    }
}