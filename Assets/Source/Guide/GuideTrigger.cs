using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
    public class GuideTrigger : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        private Button _close;
        private Transform _player;
        private Transform _enemy;
        private bool _didPlay;

        private event Action OnStarted;
        private event Action OnCompleted;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_didPlay == true)
                return;

            ShowEnemy();
        }

        public void Initialize(CinemachineVirtualCamera camera, Button close, Transform player, Transform enemy, Action onStarted, Action onCompleted)
        {
            _camera = camera;
            _close = close;
            _player = player;
            _enemy = enemy;
            OnStarted = onStarted;
            OnCompleted = onCompleted;
            
            _close.onClick.AddListener(Complete);
        }

        private void ShowEnemy()
        {
            _camera.LookAt = _enemy;
            OnStarted?.Invoke();
        }

        private void Complete()
        {
            _camera.LookAt = _player;
            OnCompleted?.Invoke();
        }
    }
}