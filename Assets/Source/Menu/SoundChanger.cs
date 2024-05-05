using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Menu
{
    public class SoundChanger : MonoBehaviour
    {
        private const string Game = nameof(Game);
        private const string Music = nameof(Music);
        private const string Environment = nameof(Environment);
        private const float MinValue = -80f;
        
        private AudioMixer _mixer;
        private Slider _game;
        private Slider _music;
        private AudioSource _sound;

        public event Action<float> GameVolumeChanged;
        public event Action<float> MusicVolumeChanged;

        private void OnDestroy()
        {
            _game.onValueChanged.RemoveListener(ChangeGame);
            _music.onValueChanged.RemoveListener(ChangeMusic);
        }

        public void Initialize(AudioMixer mixer, Slider game, Slider music, AudioSource sound)
        {
            _mixer = mixer;
            _game = game;
            _music = music;
            _sound = sound;
            
            _game.onValueChanged.AddListener(ChangeGame);
            _music.onValueChanged.AddListener(ChangeMusic);
        }

        public void Load(float gameVolume, float musicVolume)
        {
            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.GameVolume)) == true)
            {
                gameVolume = PlayerPrefs.GetFloat(nameof(CharacteristicConstants.GameVolume));
                PlayerPrefs.DeleteKey(nameof(CharacteristicConstants.GameVolume));
            }
            
            if (PlayerPrefs.HasKey(nameof(CharacteristicConstants.MusicVolume)) == true)
            {
                musicVolume = PlayerPrefs.GetFloat(nameof(CharacteristicConstants.MusicVolume));
                PlayerPrefs.DeleteKey(nameof(CharacteristicConstants.MusicVolume));
            }

            _game.value = gameVolume;
            _music.value = musicVolume;
            _sound.Play();
        }

        public void Mute()
        {
            _mixer.SetFloat(Environment, MinValue);
        }

        public void Release()
        {
            _mixer.SetFloat(Environment, (float)ValueConstants.Zero);
        }

        private void ChangeGame(float value)
        {
            _mixer.SetFloat(Game, value);
            GameVolumeChanged?.Invoke(value);
        }
        
        private void ChangeMusic(float value)
        {
            _mixer.SetFloat(Music, value);
            MusicVolumeChanged?.Invoke(value);
        }
    }
}