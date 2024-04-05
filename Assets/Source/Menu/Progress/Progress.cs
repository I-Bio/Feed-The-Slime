using System;
using System.Linq;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Progress
    {
        private readonly SerializedPair<int>[] _rewardSteps;

        private PlayerCharacteristics _characteristics;

        public Progress(PlayerCharacteristics startCharacteristics, SerializedPair<int>[] rewardSteps)
        {
            _characteristics = startCharacteristics;
            _rewardSteps = rewardSteps;
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
            _characteristics.SpitObtained = isObtained as bool? ?? throw new NullReferenceException();
        }

        public void IncreaseLevels()
        {
            _characteristics.CompletedLevels++;
            LevelsIncreased?.Invoke(_characteristics.CompletedLevels);
        }

        public void ChangeCrystals(int value)
        {
            _characteristics.CrystalsCount += value;
            CrystalsChanged?.Invoke(_characteristics.CrystalsCount);
        }

        public void RewardReceive(int value)
        {
            RewardReceived?.Invoke(value);
        }

        public void StartGame()
        {
            int rewardValue = _rewardSteps
                .Where(pair => pair.Key <= _characteristics.CompletedLevels || pair == _rewardSteps[^1])
                .Select(pair => pair.Value).FirstOrDefault();

            TransferService.Instance.SetStats(rewardValue, _characteristics);
            SceneManager.LoadScene((int)SceneNames.Game);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        public void Save()
        {
            Debug.Log("TO SAVE " + JsonUtility.ToJson(_characteristics));
            PlayerAccount.SetCloudSaveData(JsonUtility.ToJson(_characteristics));
        }

        public void Load()
        {
            PlayerAccount.GetCloudSaveData(OnLoaded);
        }

        private void OnLoaded(string jsonData)
        {
            PlayerCharacteristics characteristics = JsonUtility.FromJson<PlayerCharacteristics>(jsonData);
            
            if (characteristics != null && characteristics.Speed != 0f)
                _characteristics = characteristics;
            
            Debug.Log("PRINT AFTER LOAD " + JsonUtility.ToJson(_characteristics));
            
            Loaded?.Invoke(_characteristics);
        }
#endif
    }
}