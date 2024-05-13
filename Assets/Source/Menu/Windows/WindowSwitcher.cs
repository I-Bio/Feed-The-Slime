using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Menu
{
    [RequireComponent(typeof(Screen))]
    public class WindowSwitcher : MonoBehaviour
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

        public void ResumeScreen()
        {
            ChangeWindow(Windows.Play);
        }

        public void ShowMain()
        {
            ChangeWindow(Windows.Main);
        }

        private void ShowEvolution()
        {
            ChangeWindow(Windows.Upgrades);
        }
        
        private void PauseScreen()
        {
            ChangeWindow(Windows.Pause);
        }

        public void ChangeWindow(Windows window, Revival revival = null)
        {
            if (window == Windows.Lose && revival != null && revival.TryRevive())
                return;

            _screen.SetWindow((int)window);

            if (window != Windows.Lose && window != Windows.Win)
                return;

            if (PlayerPrefs.GetString(nameof(CharacteristicConstants.CanShowAdvert)) == string.Empty)
                return;

            InterstitialAd.Show();
            PlayerPrefs.GetString(nameof(CharacteristicConstants.CanShowAdvert), string.Empty);
        }

        private void ShowLeader()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission(() =>
                {
                    LeaderboardOpened?.Invoke();
                    _screen.SetWindow((int)Windows.Leader);
                });
                return;
            }
#endif
#if UNITY_EDITOR
            if(PlayerPrefs.HasKey(nameof(Authorize)))
                LeaderboardOpened?.Invoke();
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