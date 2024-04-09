using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(Screen))]
    public class Game : MonoBehaviour, IStageSettable
    {
        [SerializeField] private float _double = 2f;
        [SerializeField] private Button _winAdvert;
        [SerializeField] private Button _loseAdvert;
        [SerializeField] private Button[] _mainMenuButtons;
        [SerializeField] private TextMeshProUGUI[] _rewards;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _resume;
        [SerializeField] private CanvasGroup _backGround;

        private Screen _screen;

        private ITransferService _transferService;
        private Transform _player;
        private Stopper _stopper;
        private Vector3 _playerStart;
        private int _stage;
        private float _maxStage;

        public void Initialize(ITransferService transferService, Transform player)
        {
            _transferService = transferService;
            _player = player;
            _playerStart = _player.position;
            _maxStage = Enum.GetValues(typeof(SatietyStage)).Length - 1;
            SetStage(SatietyStage.Exhaustion);
            _stopper = GetComponent<Stopper>();
            _screen = GetComponent<Screen>();
        }

        public void ChangeWindow(GameWindows window)
        {
            _stopper.Pause();
            _backGround.alpha = 1f;
            _screen.SetWindow((int)window);
            
            if(window == GameWindows.Pause)
                return;

            if (_transferService.Characteristics.IsAllowedShowInter == true)
                Agava.YandexGames.InterstitialAd.Show();
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = (int)stage;

            foreach (TextMeshProUGUI reward in _rewards)
                reward.SetText(Mathf.CeilToInt(_transferService.Reward * (_stage / _maxStage)).ToString());
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
            ShowRewardAdvert(DoubleReward);
        }

        private void ShowLoseAdvert()
        {
            ShowRewardAdvert(Respawn);
        }

        private void ShowRewardAdvert(Action onReward)
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
            _transferService.MultiplyIt(_double);
        }

        private void Load()
        {
            _stopper.Release();
            _transferService.MultiplyIt(_stage / _maxStage);
            _transferService.AllowReceive();
            SceneManager.LoadScene((int)SceneNames.Menu);
        }

        private void OnScreenPause()
        {
            ChangeWindow(GameWindows.Pause);
        }

        private void OnScreenResume()
        {
            _backGround.alpha = 0f;
            _screen.Hide();
            _stopper.Release();
        }
    }
}