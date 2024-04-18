using Agava.WebUtility;
using UnityEngine;

namespace Menu
{
    public class Stopper : MonoBehaviour
    {
        private bool _isGlobalPaused;
        
        public void Pause()
        {
            _isGlobalPaused = true;
            StopTime();
        }

        public void Release()
        {
            _isGlobalPaused = false;
            StartTime();
        }
        
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
            Time.timeScale = 0f;
            AudioListener.volume = 0f;
        }

        private void StartTime()
        {
            if (_isGlobalPaused == true)
                return;
            
            Time.timeScale = 1f;
            AudioListener.volume = 1f;
        }
    }
}