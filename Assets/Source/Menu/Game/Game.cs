using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(Screen))]
    public class Game : MonoBehaviour, IGame, ILoader
    {
        [SerializeField] private float _double = 2f;
        [SerializeField] private Button _winAdvert;
        [SerializeField] private Button _loseAdvert;
        [SerializeField] private TextMeshProUGUI[] _rewards;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _resume;
        [SerializeField] private CanvasGroup _backGround;

        private Screen _screen;

        private ITransferService _transferService;
        private Revival _revival;
        private Stopper _stopper;
        private int _stage;
        private float _maxStage;

        public void Initialize(ITransferService transferService, Revival revival)
        {
            _transferService = transferService;
            _revival = revival;
            _maxStage = Enum.GetValues(typeof(SatietyStage)).Length - 1;
            SetStage(SatietyStage.Exhaustion);
            _stopper = GetComponent<Stopper>();
            _screen = GetComponent<Screen>();
        }

        public void ChangeWindow(GameWindows window)
        {
            if (window == GameWindows.Lose && _revival.TryRevive() == true)
                return;

            _stopper.Pause();
            _backGround.alpha = 1f;
            _screen.SetWindow((int)window);

            if (window == GameWindows.Pause)
                return;

            _transferService.PassLevel();

            if (_transferService.Characteristics.IsAllowedShowInter == true)
                InterstitialAd.Show();
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = (int)stage;
            UpdateReward();
        }

        public void Load()
        {
            _stopper.Release();
            _transferService.MultiplyIt(_stage / _maxStage);
            _transferService.AllowReceive();
            SceneManager.LoadScene((int)SceneNames.Menu);
        }

        private void OnEnable()
        {
            _winAdvert.onClick.AddListener(ShowWinAdvert);
            _loseAdvert.onClick.AddListener(ShowLoseAdvert);
            _pause.onClick.AddListener(OnScreenPause);
            _resume.onClick.AddListener(OnScreenResume);
        }

        private void OnDisable()
        {
            _winAdvert.onClick.RemoveListener(ShowWinAdvert);
            _loseAdvert.onClick.RemoveListener(ShowLoseAdvert);
            _pause.onClick.RemoveListener(OnScreenPause);
            _resume.onClick.RemoveListener(OnScreenResume);
        }

        private void ShowWinAdvert()
        {
            ShowRewardAdvert(DoubleReward, null);
        }

        private void ShowLoseAdvert()
        {
            ShowRewardAdvert(Respawn, OnScreenResume);
        }

        private void ShowRewardAdvert(Action onReward, Action onClose)
        {
#if UNITY_EDITOR
            onReward?.Invoke();
            onClose?.Invoke();
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            VideoAd.Show(_stopper.Pause, onReward, onClose);
#endif
        }

        private void Respawn()
        {
            _loseAdvert.gameObject.SetActive(false);
            _revival.Revive();
        }

        private void DoubleReward()
        {
            _winAdvert.gameObject.SetActive(false);
            _transferService.MultiplyIt(_double);
            UpdateReward();
        }

        private void UpdateReward()
        {
            foreach (TextMeshProUGUI reward in _rewards)
                reward.SetText(Mathf.CeilToInt(_transferService.Reward * (_stage / _maxStage)).ToString());
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