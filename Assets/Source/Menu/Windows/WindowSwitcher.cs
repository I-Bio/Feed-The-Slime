using System;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Screen))]
    public class WindowSwitcher : MonoBehaviour
    {
        [SerializeField] private Button _evolution;
        [SerializeField] private Button _leaders;
        [SerializeField] private Button _authorize;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _resume;
        [SerializeField] private Button[] _closeButtons;

        private Screen _screen;

        public event Action LeaderboardOpened;
        
        public void Initialize(Stopper stopper)
        {
            _screen = GetComponent<Screen>();
            _screen.Initialize(stopper);
        }

        private void OnEnable()
        {
            _evolution.onClick.AddListener(ShowEvolution);
            _leaders.onClick.AddListener(ShowLeader);
            _authorize.onClick.AddListener(Authorize);
            _pause.onClick.AddListener(PauseScreen);
            _resume.onClick.AddListener(ResumeScreen);
            
            foreach (Button button in _closeButtons)
                button.onClick.AddListener(ShowMain);
        }

        private void OnDisable()
        {
            _evolution.onClick.RemoveListener(ShowEvolution);
            _leaders.onClick.RemoveListener(ShowLeader);
            _authorize.onClick.RemoveListener(Authorize);
            _pause.onClick.RemoveListener(PauseScreen);
            _resume.onClick.RemoveListener(ResumeScreen);
            
            foreach (Button button in _closeButtons)
                button.onClick.RemoveListener(ShowMain);
        }
        
        public void PauseScreen()
        {
            ChangeWindow(Windows.Pause);
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
            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission(() =>
                {
                    LeaderboardOpened?.Invoke();
                    _screen.SetWindow((int)Windows.Leader);
                });
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