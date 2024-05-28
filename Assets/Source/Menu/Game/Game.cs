using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class Game : MonoBehaviour, IGame, IRewardCollector
    {
        [SerializeField] private float _minPercent = 0.01f;
        [SerializeField] private float _double = 2f;
        [SerializeField] private Button _winAdvert;
        [SerializeField] private Button _loseAdvert;
        [SerializeField] private Button[] _loadButtons;
        [SerializeField] private TextMeshProUGUI[] _rewards;

        private WindowSwitcher _switcher;
        private Stopper _stopper;
        private AutoSaveRequester _requester;
        private Revival _revival;
        private SatietyStage _stage;
        private int _rewardCount;
        private float _maxStage;
        private float _stageScale;
        private bool _didPass;

        public event Action<int, bool, Action> GoingCollect;

        private void OnDestroy()
        {
            _winAdvert.onClick.RemoveListener(ShowWinAdvert);
            _loseAdvert.onClick.RemoveListener(ShowLoseAdvert);

            foreach (Button load in _loadButtons)
                load.onClick.RemoveListener(Load);
        }

        public void Initialize(Revival revival, WindowSwitcher switcher, Stopper stopper, AutoSaveRequester requester, int rewardCount)
        {
            _revival = revival;
            _rewardCount = rewardCount;
            _maxStage = Enum.GetValues(typeof(SatietyStage)).Length - 1;
            SetStage(SatietyStage.Exhaustion);
            _switcher = switcher;
            _stopper = stopper;
            _requester = requester;

            _winAdvert.onClick.AddListener(ShowWinAdvert);
            _loseAdvert.onClick.AddListener(ShowLoseAdvert);

            foreach (Button load in _loadButtons)
                load.onClick.AddListener(Load);

            _switcher.ChangeWindow(Windows.Play);
            _requester.StartRequests();
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = stage;
            UpdateReward();
        }

        public void Win()
        {
            _didPass = true;
            _requester.StopRequests();
            _switcher.ChangeWindow(Windows.Win);
        }

        public void Lose()
        {
            _requester.StopRequests();
            
            if (_revival.TryRevive() == false)
                _switcher.ChangeWindow(Windows.Lose);
            
            _revival.Revive();
        }

        public void Load()
        {
            _rewardCount = Mathf.CeilToInt(_rewardCount * _stageScale);
            GoingCollect?.Invoke(_rewardCount, _didPass, () => { SceneManager.LoadScene((int)SceneNames.Game); });
        }

        private void ShowWinAdvert()
        {
            ShowRewardAdvert(
                onReward: () =>
                {
                    _winAdvert.gameObject.SetActive(false);
                    _rewardCount = Mathf.CeilToInt(_rewardCount * _double);
                    UpdateReward();
                },
                onClose: () => _stopper.FocusRelease(true));
        }

        private void ShowLoseAdvert()
        {
            ShowRewardAdvert(
                onReward: () => _loseAdvert.gameObject.SetActive(false),
                onClose:
                () =>
                {
                    _switcher.ResumeScreen();
                    _requester.StartRequests();
                    _stopper.FocusRelease(true);
                });
        }

        private void ShowRewardAdvert(Action onReward, Action onClose)
        {
#if UNITY_EDITOR
            onReward?.Invoke();
            onClose?.Invoke();
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            VideoAd.Show((() => _stopper.FocusPause(true)), onReward, onClose);
#endif
        }

        private void UpdateReward()
        {
            _stageScale = _stage == SatietyStage.Exhaustion ? _minPercent : (int)_stage / _maxStage;
            Debug.Log($"UPDATE REWARD\n VARIABLES:  Stage: {_stage}, StageScale_Current: {_stageScale}, " +
                      $"stageScale_Mean: {(int)_stage / _maxStage}, Reward_Mean: {_rewardCount}, " +
                      $"Reward_Current: {_rewardCount * _stageScale} & ceil {Mathf.CeilToInt(_rewardCount * _stageScale)}");
            foreach (TextMeshProUGUI reward in _rewards)
                reward.SetText(Mathf.CeilToInt(_rewardCount * _stageScale).ToString());
        }
    }
}