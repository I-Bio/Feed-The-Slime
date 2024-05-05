using System;
using Agava.WebUtility;
using UnityEngine;

namespace Menu
{
    public class Stopper : MonoBehaviour
    {
        private bool _isGamePaused;
        private Action OnPause;
        private Action OnRelease;

        private void OnEnable()
        {
            Application.focusChanged += OnFocusChangedApp;
            WebApplication.InBackgroundChangeEvent += OnFocusChangedWeb;
        }

        private void OnDisable()
        {
            Application.focusChanged -= OnFocusChangedApp;
            WebApplication.InBackgroundChangeEvent -= OnFocusChangedWeb;
        }

        public void Initialize(Action onPause, Action onRelease)
        {
            OnPause = onPause;
            OnRelease = onRelease;
        }
        
        public void Pause()
        {
            _isGamePaused = true;
            Time.timeScale = (float)ValueConstants.Zero;
            OnPause?.Invoke();
        }

        public void Release()
        {
            _isGamePaused = false;
            Time.timeScale = (float)ValueConstants.One;
            OnRelease?.Invoke();
        }

        private void OnFocusChangedApp(bool isInApp)
        {
            if (isInApp == false)
                StopTime();
            else
                StartTime();
        }

        private void OnFocusChangedWeb(bool isBackGround)
        {
            if (isBackGround == false)
                StartTime();
            else
                StopTime();
        }

        private void StopTime()
        {
            Time.timeScale = (float)ValueConstants.Zero;
            AudioListener.volume = (float)ValueConstants.Zero;
        }

        private void StartTime()
        {
            AudioListener.volume = (float)ValueConstants.One;
            
            if (_isGamePaused == true)
                return;
            
            Time.timeScale = (float)ValueConstants.One;
        }
    }
}