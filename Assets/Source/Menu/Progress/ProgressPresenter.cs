using System;
using Agava.YandexGames.Utility;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressPresenter : IPresenter
    {
        private readonly Progress Model;
        private readonly IProgressionBar[] Bars;
        private readonly Button Play;
        private readonly TextMeshProUGUI Level;
        private readonly TextMeshProUGUI Crystals;
        private readonly RewardReproducer Reward;
        private readonly WindowSwitcher Switcher;
        private readonly ObjectFiller Filler;
        private readonly YandexLeaderboard Leaderboard;
        private readonly LevelBootstrap Bootstrap;
        private readonly Stopper Stopper;
        private readonly IRewardCollector EndGame;
        private readonly SaveService SaveService;

        public ProgressPresenter(Progress model, IProgressionBar[] bars, Button play, TextMeshProUGUI level,
            TextMeshProUGUI crystals, RewardReproducer reward, WindowSwitcher switcher, ObjectFiller filler,
            YandexLeaderboard leaderboard,
            LevelBootstrap bootstrap, Stopper stopper, IRewardCollector endGame,
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
            Leaderboard = leaderboard;
            Bootstrap = bootstrap;
            Stopper = stopper;
            EndGame = endGame;
            SaveService = new SaveService(OnLoaded, characteristics);
        }

        public void Enable()
        {
            Model.CrystalsChanged += OnCrystalsChanged;
            Model.LevelsIncreased += OnLevelsIncreased;
            Model.GoingSave += OnGoingSave;
            Model.RewardPrepared += OnRewardPrepared;

            foreach (IProgressionBar bar in Bars)
                bar.Bought += OnBought;

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

            EndGame.GoingCollect -= OnGoingCollect;
            Stopper.SoundChanged -= OnSoundChanged;
            Switcher.LeaderboardOpened -= OnLeaderboardOpened;
            Play.onClick.RemoveListener(OnGameLaunched);
        }
        
        private void OnLoaded(IReadOnlyCharacteristics characteristics)
        {
            Debug.Log("Start LOAD PROGRESS");
            Model.Load(characteristics);

            Bars[(int)PurchaseNames.Speed].Initialize(characteristics.Speed);
            Bars[(int)PurchaseNames.Score].Initialize(characteristics.ScorePerEat);
            Bars[(int)PurchaseNames.Life].Initialize(characteristics.LifeCount);
            Bars[(int)PurchaseNames.Spit].Initialize(characteristics.DidObtainSpit);
            Debug.Log("BARS LOADED");
            Bootstrap.Initialize(characteristics.CompletedLevels, Stopper);
            Stopper.Load(characteristics.IsAllowedSound);
            Debug.Log("SOUND LOAD COMPLETE");
            OnLevelsIncreased(characteristics.CompletedLevels);
            OnCrystalsChanged(characteristics.CrystalsCount);
            Switcher.ShowMain();
            Debug.Log("WINDOW SWITCHED");
            Filler.EmptyUp();
            Debug.Log("END LOAD PROGRESS");
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
#if UNITY_WEBGL && !UNITY_EDITOR
            Leaderboard.SetPlayerScore(value);
            Leaderboard.Fill();
#endif
            Level.SetText(value.ToString());
        }

        private void OnGoingCollect(int rewardCount, bool didPass, Action callBack)
        {
            if (didPass == true)
                Model.IncreaseLevels();

            Model.ChangeCrystals(rewardCount);
            Reward.Reproduce(rewardCount,() => Filler.FillUp(callBack));
        }

        private void OnGoingSave(IReadOnlyCharacteristics characteristics)
        {
            SaveService.Save(characteristics);
        }

        private void OnRewardPrepared(IReadOnlyCharacteristics characteristics, int rewardCount)
        {
            Model.Save();
            Bootstrap.Launch(characteristics, Switcher, rewardCount);
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
                    Model.SetScore(value);
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