using Agava.WebUtility;
using UnityEngine;

namespace Menu
{
    public class Stopper : MonoBehaviour
    {
        public void Pause()
        {
            Time.timeScale = 0f;
            AudioListener.volume = 0f;
        }

        public void Release()
        {
            Time.timeScale = 1f;
            AudioListener.volume = 1f;
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
                Pause();
            else
                Release();
        }

        private void OnFocusChangedWeb(bool isBackGround)
        {
            if (isBackGround == false)
                Release();
            else
                Pause();
        }
    }
}