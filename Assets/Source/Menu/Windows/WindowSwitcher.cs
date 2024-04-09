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
        [SerializeField] private Button[] _closeButtons;

        private Screen _screen;

        public event Action LeaderboardOpened;
        
        private void Awake()
        {
            _screen = GetComponent<Screen>();
        }

        private void OnEnable()
        {
            _evolution.onClick.AddListener(ShowEvolution);
            _leaders.onClick.AddListener(ShowLeader);
            _authorize.onClick.AddListener(Authorize);
            
            foreach (Button button in _closeButtons)
                button.onClick.AddListener(ShowMain);
        }

        private void OnDisable()
        {
            _evolution.onClick.RemoveListener(ShowEvolution);
            _leaders.onClick.RemoveListener(ShowLeader);
            _authorize.onClick.RemoveListener(Authorize);
            
            foreach (Button button in _closeButtons)
                button.onClick.RemoveListener(ShowMain);
        }
        
        public void Hide()
        {
            _screen.Hide();
        }

        public void ShowMain()
        {
            _screen.SetWindow((int)MainMenuWindows.Main);
        }
        
        private void ShowEvolution()
        {
            _screen.SetWindow((int)MainMenuWindows.Evolution);
        }

        private void ShowLeader()
        {
            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission(() =>
                {
                    LeaderboardOpened?.Invoke();
                    _screen.SetWindow((int)MainMenuWindows.Leader);
                });
                return;
            }

            _screen.SetWindow((int)MainMenuWindows.Authorize);
        }

        private void Authorize()
        {
            PlayerAccount.Authorize();
            ShowMain();
        }
    }
}