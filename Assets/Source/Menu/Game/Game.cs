using System;
using Players;
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
        private Advert _advert;
        private Revival _revival;
        private SatietyStage _stage;
        private int _rewardCount;
        private float _maxStage;
        private float _stageScale;
        private bool _didPass;

        public event Action<int, bool, Action> GoingCollect;

        private void OnDestroy()
        {
            _winAdvert.onClick.RemoveListener(OnWinAdvert);
            _loseAdvert.onClick.RemoveListener(OnLoseAdvert);

            foreach (Button load in _loadButtons)
                load.onClick.RemoveListener(Load);
        }

        public void Initialize(
            Revival revival,
            WindowSwitcher switcher,
            Stopper stopper,
            AutoSaveRequester requester,
            int rewardCount)
        {
            _revival = revival;
            _switcher = switcher;
            _requester = requester;
            _rewardCount = rewardCount;
            _stopper = stopper;
            _maxStage = Enum.GetValues(typeof(SatietyStage)).Length - 1;
            SetStage(SatietyStage.Exhaustion);
            _advert = new Advert(_stopper);

            _winAdvert.onClick.AddListener(OnWinAdvert);
            _loseAdvert.onClick.AddListener(OnLoseAdvert);

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
        }

        private void Load()
        {
            _rewardCount = Mathf.CeilToInt(_rewardCount * _stageScale);
            GoingCollect?.Invoke(_rewardCount, _didPass, () => SceneManager.LoadScene((int)SceneNames.Game));
        }

        private void OnWinAdvert()
        {
            _advert.ShowReward(
                onReward: () =>
                {
                    _winAdvert.gameObject.SetActive(false);
                    _rewardCount = Mathf.CeilToInt(_rewardCount * _double);
                    UpdateReward();
                },
                onClose: () => _stopper.FocusRelease(true));
        }

        private void OnLoseAdvert()
        {
            _revival.Revive();

            _advert.ShowReward(
                onReward: () =>
                {
                    _loseAdvert.gameObject.SetActive(false);
                    _switcher.ResumeScreen();
                    _requester.StartRequests();
                },
                onClose:
                () => _stopper.FocusRelease(true));
        }

        private void UpdateReward()
        {
            _stageScale = _stage == SatietyStage.Exhaustion ? _minPercent : (int)_stage / _maxStage;

            foreach (TextMeshProUGUI reward in _rewards)
                reward.SetText(Mathf.CeilToInt(_rewardCount * _stageScale).ToString());
        }
    }
}