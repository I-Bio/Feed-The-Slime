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

        private Action _onStartedCallback;
        private Action _onCompletedCallback;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_didPlay == true)
                return;

            ShowEnemy();
        }

        public void Initialize(CinemachineVirtualCamera camera, Button close, Transform player, Transform enemy,
            Action onStartedCallback, Action onCompletedCallback)
        {
            _camera = camera;
            _close = close;
            _player = player;
            _enemy = enemy;
            _onStartedCallback = onStartedCallback;
            _onCompletedCallback = onCompletedCallback;
            
            _close.onClick.AddListener(Complete);
        }

        private void ShowEnemy()
        {
            _didPlay = true;
            _camera.LookAt = _enemy;
            _onStartedCallback?.Invoke();
        }

        private void Complete()
        {
            _camera.LookAt = _player;
            _onCompletedCallback?.Invoke();
        }
    }
}