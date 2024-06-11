using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Screen))]
    public class WindowSwitcher : MonoBehaviour, IReturnSwitcher
    {
        private Button _upgrade;
        private Button _leader;
        private Button _authorize;
        private Button _pause;
        private Button _resume;
        private Button _volume;
        private Button[] _closeButtons;
        private Screen _screen;

        public event Action LeaderboardOpened;

        private void OnDestroy()
        {
            _upgrade.onClick.RemoveListener(ShowEvolution);
            _leader.onClick.RemoveListener(ShowLeader);
            _authorize.onClick.RemoveListener(Authorize);
            _pause.onClick.RemoveListener(PauseScreen);
            _resume.onClick.RemoveListener(ResumeScreen);
            
            foreach (Button button in _closeButtons)
                button.onClick.RemoveListener(ShowMain);
        }
        
        public void Initialize(Stopper stopper, Button upgrade, Button leader, Button authorize, Button pause,
            Button resume, Button[] closeButtons)
        {
            _upgrade = upgrade;
            _leader = leader;
            _authorize = authorize;
            _pause = pause;
            _resume = resume;
            _closeButtons = closeButtons;
            
            _screen = GetComponent<Screen>();
            _screen.Initialize(stopper);
            
            _upgrade.onClick.AddListener(ShowEvolution);
            _leader.onClick.AddListener(ShowLeader);
            _authorize.onClick.AddListener(Authorize);
            _pause.onClick.AddListener(PauseScreen);
            _resume.onClick.AddListener(ResumeScreen);

            foreach (Button button in _closeButtons)
                button.onClick.AddListener(ShowMain);
        }

        public void ChangeWindow(Windows window)
        {
            _screen.SetWindow((int)window);
        }

        public void ResumeScreen()
        {
            ChangeWindow(Windows.Play);
        }

        public void ShowMain()
        {
            ChangeWindow(Windows.Main);
        }
        
        public void OpenWarning()
        {
            ChangeWindow(Windows.ExitWarning);
        }

        public void AcceptWarning()
        {
            ChangeWindow(Windows.Exit);
        }

        public void DeclineWarning()
        {
            ChangeWindow(Windows.Pause);
        }

        private void ShowEvolution()
        {
            ChangeWindow(Windows.Upgrades);
        }
        
        private void PauseScreen()
        {
            ChangeWindow(Windows.Pause);
        }

        private void ShowLeader()
        {
            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
                LeaderboardOpened?.Invoke();
                _screen.SetWindow((int)Windows.Leader);
                return;
            }

            _screen.SetWindow((int)Windows.Authorize);
        }

        private void Authorize()
        {
            PlayerAccount.Authorize();
            ShowMain();
        }
    }
}