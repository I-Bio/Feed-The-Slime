﻿using System;
using Agava.YandexGames;
using UnityEngine;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Menu
{
    public class SaveService
    {
        private readonly Action<IReadOnlyCharacteristics> OnFinished;
        
        private IReadOnlyCharacteristics _characteristics;
        
        public SaveService(Action<IReadOnlyCharacteristics> onFinished, IReadOnlyCharacteristics startCharacteristics)
        {
            OnFinished = onFinished;
            _characteristics = startCharacteristics;
        }
        
        public void Save(IReadOnlyCharacteristics characteristics)
        {
#if UNITY_EDITOR
            PlayerPrefs.SetString(nameof(PlayerCharacteristics), JsonUtility.ToJson(characteristics));
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerAccount.SetCloudSaveData(JsonUtility.ToJson(characteristics));
#endif
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.Reward)) == false)
                PlayerPrefs.SetInt(nameof(CharacteristicConstants.Reward), int.MinValue);
            
            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.CanShowAdvert)) == false)
                PlayerPrefs.SetString(nameof(CharacteristicConstants.CanShowAdvert), string.Empty);

            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.DidPassGuide)) == false)
                PlayerPrefs.SetString(nameof(CharacteristicConstants.DidPassGuide), string.Empty);
#if UNITY_EDITOR
            OnLoaded(PlayerPrefs.GetString(nameof(PlayerCharacteristics)));
#endif     
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerAccount.GetCloudSaveData(OnLoaded);
#endif
        }
        
        private void OnLoaded(string jsonData)
        {
            PlayerCharacteristics characteristics = JsonUtility.FromJson<PlayerCharacteristics>(jsonData);

            if (characteristics != null && characteristics.Speed >= _characteristics.Speed)
                _characteristics = JsonUtility.FromJson<PlayerCharacteristics>(jsonData);

            OnFinished?.Invoke(_characteristics);
        }
    }
}