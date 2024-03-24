using Cinemachine;
using UnityEngine;

namespace Players
{
    public class SizeScaler : MonoBehaviour
    {
        private Transform _transform;
        private CinemachineTransposer _virtualCamera;
        private float _scaleFactor;
        private float _cameraScale;
        private Vector3 _startScale;
        private Vector3 _startOffset;

        public void Initialize(Transform transform, CinemachineVirtualCamera virtualCamera, float scaleFactor, float cameraScale)
        {
            _transform = transform;
            _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _scaleFactor = scaleFactor;
            _cameraScale = cameraScale;
            _startScale = _transform.localScale;
            _startOffset = _virtualCamera.m_FollowOffset;
        }

        public void Scale(SatietyStage stage)
        {
            _transform.localScale = _startScale * (_scaleFactor * (int)stage);
            _virtualCamera.m_FollowOffset = _startOffset * (_cameraScale * (int)stage);
        }
    }
}