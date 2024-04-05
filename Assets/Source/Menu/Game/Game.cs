using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(Screen))]
    public class Game : MonoBehaviour, IGame
    {
        [SerializeField] private float _double = 2f;
        [SerializeField] private Button _winAdvert;
        [SerializeField] private Button _loseAdvert;
        [SerializeField] private Button[] _mainMenuButtons;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _resume;
        [SerializeField] private CanvasGroup _backGround;
        [SerializeField] private HideOption _option;

        private Screen _screen;

        private ITransferService _rewardService;
        private Transform _player;
        private Stopper _stopper;
        private Vector3 _playerStart;

        public void Initialize(ITransferService rewardService, Transform player)
        {
            _rewardService = rewardService;
            _player = player;
            _playerStart = _player.position;
            _stopper = GetComponent<Stopper>();
            _screen = GetComponent<Screen>();
        }

        public void ChangeWindow(GameWindows window)
        {
            _stopper.Pause();
            _backGround.alpha = 1f;
            _screen.SetWindow((int)window);
        }
        
        private void OnEnable()
        {
            _winAdvert.onClick.AddListener(ShowWinAdvert);
            _loseAdvert.onClick.AddListener(ShowLoseAdvert);
            _pause.onClick.AddListener(OnScreenPause);
            _resume.onClick.AddListener(OnScreenResume);

            foreach (Button button in _mainMenuButtons)
                button.onClick.AddListener(Load);
        }

        private void OnDisable()
        {
            _winAdvert.onClick.RemoveListener(ShowWinAdvert);
            _loseAdvert.onClick.RemoveListener(ShowLoseAdvert);
            _pause.onClick.RemoveListener(OnScreenPause);
            _resume.onClick.RemoveListener(OnScreenResume);
            
            foreach (Button button in _mainMenuButtons)
                button.onClick.RemoveListener(Load);
        }

        private void ShowWinAdvert()
        {
            Show(DoubleReward);
        }
        
        private void ShowLoseAdvert()
        {
            Show(Respawn);
        }
        
        private void Show(Action onReward)
        {
            Agava.YandexGames.VideoAd.Show(_stopper.Pause, onReward, _stopper.Release);
        }

        private void Respawn()
        {
            _player.position = _playerStart;
            OnScreenResume();
        }

        private void DoubleReward()
        {
            _rewardService.MultiplyIt(_double);
        }

        private void Load()
        {
            _stopper.Release();
            _rewardService.AllowReceive();
            SceneManager.LoadScene((int)SceneNames.Menu);
        }

        private void OnScreenPause()
        {
            ChangeWindow(GameWindows.Pause);
        }
        
        private void OnScreenResume()
        {
            _backGround.alpha = 0f;
            _screen.HideWindow(_option);
            _stopper.Release();
        }
    }
}