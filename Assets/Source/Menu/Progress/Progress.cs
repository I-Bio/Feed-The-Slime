using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Progress
    {
        private readonly SerializedPair<int, int>[] RewardSteps;
        private readonly int AdvertAccumulationStep;

        private PlayerCharacteristics _characteristics;

        public Progress(PlayerCharacteristics startCharacteristics, SerializedPair<int, int>[] rewardSteps,
            int advertAccumulationStep)
        {
            _characteristics = startCharacteristics;
            RewardSteps = rewardSteps;
            AdvertAccumulationStep = advertAccumulationStep;
        }

        public event Action<PlayerCharacteristics> GoingSave;
        public event Action<int> CrystalsChanged;
        public event Action<int> LevelsIncreased;
        public event Action<IReadOnlyCharacteristics, int> RewardPrepared;

        public void SetSpeed(object speed)
        {
            _characteristics.Speed = speed as float? ?? throw new NullReferenceException();
        }

        public void SetScorePerEat(object score)
        {
            _characteristics.ScorePerEat = score as float? ?? throw new NullReferenceException();
        }

        public void SetLifeCount(object lifeCount)
        {
            _characteristics.LifeCount = lifeCount as int? ?? throw new NullReferenceException();
        }

        public void ObtainSpit(object isObtained)
        {
            _characteristics.DidObtainSpit = isObtained as bool? ?? throw new NullReferenceException();
        }

        public void IncreaseLevels()
        {
            _characteristics.CompletedLevels++;
            LevelsIncreased?.Invoke(_characteristics.CompletedLevels);
        }

        public void AccumulateAdvert()
        {
            _characteristics.IsAllowedShowInter = false;
            _characteristics.AdvertAccumulation++;

            if (_characteristics.AdvertAccumulation != AdvertAccumulationStep)
                return;

            _characteristics.AdvertAccumulation = (int)ValueConstants.Zero;
            _characteristics.IsAllowedShowInter = true;
        }

        public void ChangeScore(float score)
        {
            _characteristics.ProgressScore = score;
            PlayerPrefs.SetFloat(nameof(PlayerCharacteristics.ProgressScore), _characteristics.ProgressScore);
            PlayerPrefs.Save();
        }

        public void ChangeCrystals(int value)
        {
            _characteristics.CrystalsCount += value;
            CrystalsChanged?.Invoke(_characteristics.CrystalsCount);
        }

        public void PrepareReward()
        {
            int rewardValue = RewardSteps.Last(pair => pair.Key <= _characteristics.CompletedLevels).Value;
            RewardPrepared?.Invoke(_characteristics, rewardValue);
        }
        
        public void Load(IReadOnlyCharacteristics characteristics)
        {
            _characteristics = characteristics as PlayerCharacteristics;

            if (_characteristics == null)
                throw new NullReferenceException(nameof(_characteristics));

            float prefsScore = PlayerPrefs.HasKey(nameof(PlayerCharacteristics.ProgressScore))
                ? PlayerPrefs.GetFloat(nameof(PlayerCharacteristics.ProgressScore))
                : (float)ValueConstants.Zero;
            _characteristics.ProgressScore = Mathf.Max(_characteristics.ProgressScore, prefsScore);
            _characteristics.IsAllowedSound = PlayerPrefs.HasKey(nameof(PlayerCharacteristics.IsAllowedSound))
                ? Convert.ToBoolean(PlayerPrefs.GetString(nameof(PlayerCharacteristics.IsAllowedSound)))
                : _characteristics.IsAllowedSound;

            if (_characteristics.DidPassGuide == true)
                return;

            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.DidPassGuide)) == false)
            {
                SceneManager.LoadScene((int)SceneNames.Guide);
                return;
            }
                
            _characteristics.DidPassGuide = true;
        }
        
        public void Save()
        {
            GoingSave?.Invoke(_characteristics);
        }

        public void ChangeSound(bool isAllowed)
        {
            _characteristics.IsAllowedSound = isAllowed;
        }

        public void UpdateLeaderboardScore(Action<int> onUpdated)
        {
            onUpdated?.Invoke(_characteristics.CompletedLevels);
        }
    }
}