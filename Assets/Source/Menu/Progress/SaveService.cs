using System;
using Agava.YandexGames;
using UnityEngine;

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
            if (PlayerPrefs.HasKey(nameof(PlayerCharacteristics.IsAllowedSound)) == true)
                PlayerPrefs.DeleteKey(nameof(PlayerCharacteristics.IsAllowedSound));
            

            PlayerPrefs.Save();
            PlayerAccount.SetCloudSaveData(JsonUtility.ToJson(characteristics));
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.CanShowAdvert)) == false)
                PlayerPrefs.SetString(nameof(CharacteristicConstants.CanShowAdvert), string.Empty);
            
            PlayerAccount.GetCloudSaveData(OnLoaded);
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