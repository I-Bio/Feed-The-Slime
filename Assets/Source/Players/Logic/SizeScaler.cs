using System;
using Cinemachine;
using UnityEngine;

namespace Players
{
    public class SizeScaler : MonoBehaviour
    {
        private Transform _player;
        private Transform _camera;
        private CinemachineTransposer _virtualCamera;
        private float _scaleFactor;
        private float _cameraScale;
        private Vector3 _startScale;
        private Vector3 _startOffset;

        public void Initialize(Transform player, CinemachineVirtualCamera virtualCamera, float scaleFactor, float cameraScale)
        {
            _player = player;
            _camera = virtualCamera.transform;
            _virtualCamera = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _scaleFactor = scaleFactor;
            _cameraScale = cameraScale;
            _startScale = _player.localScale;
            _startOffset = _virtualCamera.m_FollowOffset;
        }

        public void Scale(SatietyStage stage, Action<Vector3> onScaledCallback = null)
        {
            if (stage == SatietyStage.Exhaustion)
                return;
            
            _player.localScale = _startScale * (_scaleFactor * (int)stage);
            Vector3 newOffset = _startOffset * (_cameraScale * (int)stage);
            onScaledCallback?.Invoke(_camera.position - _virtualCamera.m_FollowOffset + newOffset);
            _virtualCamera.m_FollowOffset = newOffset;
        }
    }
}