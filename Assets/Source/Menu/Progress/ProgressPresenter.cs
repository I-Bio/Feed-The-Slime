using System;
using Agava.YandexGames.Utility;
using Spawners;
using TMPro;
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
        private readonly SoundChanger Sound;
        private readonly IRewardCollector EndGame;
        private readonly SaveService SaveService;

        public ProgressPresenter(Progress model, IProgressionBar[] bars, Button play, TextMeshProUGUI level,
            TextMeshProUGUI crystals, RewardReproducer reward, WindowSwitcher switcher, ObjectFiller filler,
            YandexLeaderboard leaderboard,
            LevelBootstrap bootstrap, Stopper stopper, SoundChanger sound, IRewardCollector endGame,
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
            Sound = sound;
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
            Sound.GameVolumeChanged += OnGameVolumeChanged;
            Sound.MusicVolumeChanged += OnMusicVolumeChanged;
#if UNITY_WEBGL && !UNITY_EDITOR
            Switcher.LeaderboardOpened += OnLeaderboardOpened;
#endif
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
            Sound.GameVolumeChanged -= OnGameVolumeChanged;
            Sound.MusicVolumeChanged -= OnMusicVolumeChanged;
#if UNITY_WEBGL && !UNITY_EDITOR
            Switcher.LeaderboardOpened -= OnLeaderboardOpened;
#endif
            Play.onClick.RemoveListener(OnGameLaunched);
        }
        
        private void OnLoaded(IReadOnlyCharacteristics characteristics)
        {
            Model.Load(characteristics);

            Bars[(int)PurchaseNames.Speed].Initialize(characteristics.Speed);
            Bars[(int)PurchaseNames.Score].Initialize(characteristics.ScorePerEat);
            Bars[(int)PurchaseNames.Life].Initialize(characteristics.LifeCount);
            Bars[(int)PurchaseNames.Spit].Initialize(characteristics.DidObtainSpit);

            Bootstrap.Initialize(characteristics.CompletedLevels);
            Sound.Load(characteristics.GameVolume, characteristics.MusicVolume);
            OnLevelsIncreased(characteristics.CompletedLevels);
            OnCrystalsChanged(characteristics.CrystalsCount);
            Switcher.ShowMain();
            Filler.EmptyUp();
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

        private void OnGoingCollect(int rewardCount, bool didPass, bool haveReward, Action callBack)
        {
            if (didPass == true)
                Model.IncreaseLevels();

            if (haveReward == true)
            {
                Model.ChangeCrystals(rewardCount);
                Reward.Reproduce(() => Filler.FillUp(callBack));
                return;
            }

            Filler.FillUp(callBack);
        }

        private void OnGoingSave(IReadOnlyCharacteristics characteristics)
        {
            SaveService.Save(characteristics);
        }

        private void OnRewardPrepared(IReadOnlyCharacteristics characteristics, int rewardCount)
        {
            Model.Save();
            Bootstrap.Launch(characteristics, Switcher, Stopper, rewardCount);
        }

        private void OnGameVolumeChanged(float volume)
        {
            Model.ChangeGameVolume(volume);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            Model.ChangeMusicVolume(volume);
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

#if UNITY_WEBGL && !UNITY_EDITOR
        private void OnLeaderboardOpened()
        {
            Model.UpdateLeaderboardScore(OnLevelsIncreased);
        }
#endif
    }
}