using System;
using TMPro;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressPresenter
    {
        private readonly Progress Model;
        private readonly IProgressionBar[] Bars;
        private readonly Button Play;
        private readonly TextMeshProUGUI Level;
        private readonly TextMeshProUGUI Crystals;
        private readonly RewardReproducer Reward;
        private readonly WindowSwitcher Switcher;
        private readonly ObjectFiller Filler;
        private readonly AutoSaveRequester Requester;
        private readonly YandexLeaderboard Leaderboard;
        private readonly LevelBootstrap Bootstrap;
        private readonly Stopper Stopper;
        private readonly IRewardCollector EndGame;
        private readonly SaveService SaveService;
        private readonly Advert Advert;
        private readonly SDKReadyCaller Caller;

        public ProgressPresenter(
            Progress model,
            IProgressionBar[] bars,
            Button play,
            TextMeshProUGUI level,
            TextMeshProUGUI crystals,
            RewardReproducer reward,
            WindowSwitcher switcher,
            ObjectFiller filler,
            AutoSaveRequester requester,
            YandexLeaderboard leaderboard,
            LevelBootstrap bootstrap,
            Stopper stopper,
            IRewardCollector endGame,
            PlayerCharacteristics characteristics)
        {
            Model = model;
            Bars = bars;
            Play = play;
            Level = level;
            Crystals = crystals;
            Reward = reward;
            Switcher = switcher;
            Filler = filler;
            Requester = requester;
            Leaderboard = leaderboard;
            Bootstrap = bootstrap;
            Stopper = stopper;
            EndGame = endGame;
            SaveService = new SaveService(OnLoaded, characteristics);
            Advert = new Advert(Stopper);
            Caller = new SDKReadyCaller();
        }

        public void Enable()
        {
            Model.CrystalsChanged += OnCrystalsChanged;
            Model.LevelsIncreased += OnLevelsIncreased;
            Model.GoingSave += OnGoingSave;
            Model.RewardPrepared += OnRewardPrepared;

            foreach (IProgressionBar bar in Bars)
                bar.Bought += OnBought;

            Requester.SaveRequested += OnSaveRequested;
            EndGame.GoingCollect += OnGoingCollect;
            Stopper.SoundChanged += OnSoundChanged;
            Switcher.LeaderboardOpened += OnLeaderboardOpened;
            Play.onClick.AddListener(OnGameLaunched);

            SaveService.Load();
        }

        public void Disable()
        {
            Model.CrystalsChanged -= OnCrystalsChanged;
            Model.LevelsIncreased -= OnLevelsIncreased;
            Model.GoingSave -= OnGoingSave;
            Model.RewardPrepared -= OnRewardPrepared;

            foreach (IProgressionBar bar in Bars)
                bar.Bought -= OnBought;

            Requester.SaveRequested -= OnSaveRequested;
            EndGame.GoingCollect -= OnGoingCollect;
            Stopper.SoundChanged -= OnSoundChanged;
            Switcher.LeaderboardOpened -= OnLeaderboardOpened;
            Play.onClick.RemoveListener(OnGameLaunched);
        }

        private void OnLoaded(IReadOnlyCharacteristics characteristics)
        {
            Model.Load(characteristics);

            if (characteristics.IsAllowedShowInter)
                Advert.ShowInter();

            Bars[(int)PurchaseNames.Speed].Initialize(characteristics.Speed);
            Bars[(int)PurchaseNames.Score].Initialize(characteristics.ScorePerEat);
            Bars[(int)PurchaseNames.Life].Initialize(characteristics.LifeCount);
            Bars[(int)PurchaseNames.Spit].Initialize(characteristics.DidObtainSpit);

            Switcher.ShowMain();
            Bootstrap.Initialize(characteristics.CompletedLevels);
            Stopper.Load(characteristics.IsAllowedSound);
            OnLevelsIncreased(characteristics.CompletedLevels);
            OnCrystalsChanged(characteristics.CrystalsCount);
            Filler.EmptyUp();
            Caller.CallGameReady();
        }

        private void OnCrystalsChanged(int crystalsCount)
        {
            Model.Save();
            Crystals.SetText(crystalsCount.ToString());

            foreach (IProgressionBar bar in Bars)
                bar.CompareCrystals(crystalsCount);
        }

        private void OnLevelsIncreased(int value)
        {
            Leaderboard.SetPlayerScore(value);
            Leaderboard.Fill();
            Level.SetText(value.ToString());
        }

        private void OnSaveRequested()
        {
            Model.Save();
        }

        private void OnGoingCollect(int rewardCount, bool didPass, Action callBack)
        {
            if (didPass == true)
            {
                Model.IncreaseLevels();
                Model.ChangeScore((float)ValueConstants.Zero);
            }

            Model.AccumulateAdvert();
            Model.ChangeCrystals(rewardCount);
            Reward.Reproduce(rewardCount, () => Filler.FillUp(callBack));
        }

        private void OnGoingSave(IReadOnlyCharacteristics characteristics)
        {
            SaveService.Save(characteristics);
        }

        private void OnRewardPrepared(IReadOnlyCharacteristics characteristics, int rewardCount)
        {
            Model.Save();
            Bootstrap.Launch(characteristics, rewardCount, Model.ChangeScore);
        }

        private void OnSoundChanged(bool isAllowed)
        {
            Model.ChangeSound(isAllowed);
        }

        private void OnBought(int spendCount, PurchaseNames progression, object value)
        {
            switch (progression)
            {
                case PurchaseNames.Speed:
                    Model.SetSpeed(value);
                    break;

                case PurchaseNames.Score:
                    Model.SetScorePerEat(value);
                    break;

                case PurchaseNames.Life:
                    Model.SetLifeCount(value);
                    break;

                case PurchaseNames.Spit:
                    Model.ObtainSpit(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(progression), progression, null);
            }

            Model.ChangeCrystals(-spendCount);
        }

        private void OnGameLaunched()
        {
            Model.PrepareReward();
        }

        private void OnLeaderboardOpened()
        {
            Model.UpdateLeaderboardScore(OnLevelsIncreased);
        }
    }
}