using System;
using System.Linq;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Progress
    {
        private readonly SerializedPair<int, int>[] _rewardSteps;
        private readonly int _advertAccumulationStep;

        private PlayerCharacteristics _characteristics;

        public Progress(PlayerCharacteristics startCharacteristics, SerializedPair<int, int>[] rewardSteps,
            int advertAccumulationStep)
        {
            _characteristics = startCharacteristics;
            _rewardSteps = rewardSteps;
            _advertAccumulationStep = advertAccumulationStep;
        }

        public event Action<IReadOnlyCharacteristics> Loaded;
        public event Action<int> CrystalsChanged;
        public event Action<int> RewardReceived;
        public event Action<int> LevelsIncreased;

        public void SetSpeed(object speed)
        {
            _characteristics.Speed = speed as float? ?? throw new NullReferenceException();
        }

        public void SetScore(object score)
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

            _characteristics.IsAllowedShowInter = false;
            _characteristics.AdvertAccumulation++;

            if (_characteristics.AdvertAccumulation != _advertAccumulationStep)
                return;

            _characteristics.AdvertAccumulation = 0;
            _characteristics.IsAllowedShowInter = true;
        }

        public void ChangeCrystals(int value)
        {
            _characteristics.CrystalsCount += value;
            CrystalsChanged?.Invoke(_characteristics.CrystalsCount);
        }

        public void RewardReceive(int value)
        {
            CompleteGuide();
            ChangeCrystals(value);
        }

        public void StartGame()
        {
            int rewardValue = _rewardSteps
                .Where(pair => pair.Key <= _characteristics.CompletedLevels || pair == _rewardSteps[^1])
                .Select(pair => pair.Value).FirstOrDefault();

            TransferService.Instance.SetStats(rewardValue, _characteristics);

            if (_characteristics.DidPassGuide == true)
                SceneManager.LoadScene((int)SceneNames.Game);
            else
                SceneManager.LoadScene((int)SceneNames.Guide);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        public void UpdateLeaderboardScore(Action<int> onUpdated)
        {
            onUpdated?.Invoke(_characteristics.CompletedLevels);
        }
        
        public void Save()
        {
            PlayerAccount.SetCloudSaveData(JsonUtility.ToJson(_characteristics));
        }

        public void Load()
        {
            PlayerAccount.GetCloudSaveData(OnLoaded);
        }

        private void OnLoaded(string jsonData)
        {
            PlayerCharacteristics characteristics = JsonUtility.FromJson<PlayerCharacteristics>(jsonData);

            if (characteristics != null && characteristics.Speed >= _characteristics.Speed)
                _characteristics = JsonUtility.FromJson<PlayerCharacteristics>(jsonData);

            Loaded?.Invoke(_characteristics);
        }
#endif
        private void CompleteGuide()
        {
            if (_characteristics.DidPassGuide == true)
                return;

            _characteristics.DidPassGuide = true;
        }
    }
}