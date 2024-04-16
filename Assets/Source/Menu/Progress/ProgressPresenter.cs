using System;
using TMPro;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressPresenter : IPresenter
    {
        private readonly Progress _model;
        private readonly IProgressionBar[] _bars;
        private readonly Button _play;
        private readonly TextMeshProUGUI _level;
        private readonly TextMeshProUGUI _crystals;
        private readonly RewardReproducer _reward;
        private readonly WindowSwitcher _switcher;
        private readonly YandexLeaderboard _leaderboard;
        private readonly TransferService _transferService;

        public ProgressPresenter(Progress model, IProgressionBar[] bars, Button play, TextMeshProUGUI level,
            TextMeshProUGUI crystals, RewardReproducer reward, WindowSwitcher switcher, YandexLeaderboard leaderboard,
            TransferService transferService)
        {
            _model = model;
            _bars = bars;
            _play = play;
            _level = level;
            _crystals = crystals;
            _reward = reward;
            _switcher = switcher;
            _leaderboard = leaderboard;
            _transferService = transferService;
        }

        public void Enable()
        {
            _model.Loaded += OnLoaded;
            _model.CrystalsChanged += OnCrystalsChanged;
            _model.RewardReceived += OnRewardReceived;
            _model.LevelsIncreased += OnLevelsIncreased;
#if UNITY_WEBGL && !UNITY_EDITOR
            _switcher.LeaderboardOpened += OnLeaderboardOpened;
#endif
            foreach (IProgressionBar bar in _bars)
                bar.Bought += OnBought;

            _play.onClick.AddListener(_model.StartGame);
        }

        public void Disable()
        {
            _model.Loaded -= OnLoaded;
            _model.CrystalsChanged -= OnCrystalsChanged;
            _model.RewardReceived -= OnRewardReceived;
            _model.LevelsIncreased -= OnLevelsIncreased;
#if UNITY_WEBGL && !UNITY_EDITOR
            _switcher.LeaderboardOpened -= OnLeaderboardOpened;
#endif
            foreach (IProgressionBar bar in _bars)
                bar.Bought -= OnBought;

            _play.onClick.RemoveListener(_model.StartGame);
        }

        private void OnLoaded(IReadOnlyCharacteristics characteristics)
        {
            _bars[(int)PurchaseNames.Speed].Initialize(characteristics.Speed);
            _bars[(int)PurchaseNames.Score].Initialize(characteristics.ScorePerEat);
            _bars[(int)PurchaseNames.Life].Initialize(characteristics.LifeCount);
            _bars[(int)PurchaseNames.Spit].Initialize(characteristics.DidObtainSpit);

            OnLevelsIncreased(characteristics.CompletedLevels);

            if (_transferService.DidLevelPassed == true)
                _model.AccumulateInter();

            OnCrystalsChanged(characteristics.CrystalsCount);
            _switcher.ShowMain();
            
            if (_transferService.TryGetReward(out int value) == false)
                return;

            _switcher.Hide();
            _reward.Reproduce(_switcher.ShowMain);
            _model.RewardReceive(value);
        }

        private void OnCrystalsChanged(int crystalsCount)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _model.Save();
#endif
            _crystals.SetText(crystalsCount.ToString());

            foreach (IProgressionBar bar in _bars)
                bar.CompareCrystals(crystalsCount);
        }

        private void OnRewardReceived(int value)
        {
            _model.IncreaseLevels();
            _model.ChangeCrystals(value);
        }

        private void OnLevelsIncreased(int value)
        {
            _leaderboard.SetPlayerScore(value);
            _leaderboard.Fill();
            _level.SetText(value.ToString());
        }

        private void OnBought(int spendCount, PurchaseNames progression, object value)
        {
            switch (progression)
            {
                case PurchaseNames.Speed:
                    _model.SetSpeed(value);
                    break;

                case PurchaseNames.Score:
                    _model.SetScore(value);
                    break;

                case PurchaseNames.Life:
                    _model.SetLifeCount(value);
                    break;

                case PurchaseNames.Spit:
                    _model.ObtainSpit(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(progression), progression, null);
            }

            _model.ChangeCrystals(-spendCount);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        private void OnLeaderboardOpened()
        {
            _model.UpdateLeaderboardScore(OnLevelsIncreased);
        }
#endif
    }
}