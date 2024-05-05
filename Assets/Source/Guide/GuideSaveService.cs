using System;
using Menu;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Guide
{
    public class GuideSaveService : SaveService
    {
        public GuideSaveService(Action<IReadOnlyCharacteristics> onFinished,
            IReadOnlyCharacteristics startCharacteristics) : base(onFinished, startCharacteristics) {}
        
        public void OnGameVolumeChanged(float volume)
        {
            PlayerPrefs.SetFloat(nameof(CharacteristicConstants.GameVolume), volume);
        }
        
        public void OnMusicVolumeChanged(float volume)
        {
            PlayerPrefs.SetFloat(nameof(CharacteristicConstants.MusicVolume), volume);
        }
    }
}