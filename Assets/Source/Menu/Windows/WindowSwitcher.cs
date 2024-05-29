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
        private Stopper _stopper;

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
            _stopper = stopper;
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
#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission(() =>
                {
                    LeaderboardOpened?.Invoke();
                },
                _ => LeaderboardOpened?.Invoke());
                _screen.SetWindow((int)Windows.Leader);
                return;
            }
#endif
#if UNITY_EDITOR
            if (PlayerPrefs.HasKey(nameof(Authorize)))
            {
                LeaderboardOpened?.Invoke();
                _screen.SetWindow((int)Windows.Leader);
                return;
            }
#endif
            _screen.SetWindow((int)Windows.Authorize);
        }

        private void Authorize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerAccount.Authorize();
#endif
#if UNITY_EDITOR
            PlayerPrefs.SetString(nameof(Authorize), nameof(Authorize));
#endif
            ShowMain();
        }
    }
}