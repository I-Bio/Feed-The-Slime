using System;
using System.Collections.Generic;
using System.Linq;
using Agava.WebUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Stopper : MonoBehaviour
    {
        private bool _isGamePause;
        private bool _isForcePause;
        private bool _isAllowed = true;
        private Sprite _onIcon;
        private Sprite _offIcon;
        private Button _volume;
        private Image _icon;
        private List<AudioSource> _sources;
        private AudioSource _music;

        public event Action<bool> SoundChanged;
        
        private void OnDestroy()
        {
            Application.focusChanged -= OnFocusChangedApp;
            WebApplication.InBackgroundChangeEvent -= OnFocusChangedWeb;
            
            _volume.onClick.RemoveListener(ChangeSound);
        }

        public void Initialize(Sprite onIcon, Sprite offIcon, Button volume, Image icon,
            List<AudioSource> sources, AudioSource music)
        {
            _onIcon = onIcon;
            _offIcon = offIcon;
            _volume = volume;
            _icon = icon;
            _sources = sources;
            _music = music;

            Application.focusChanged += OnFocusChangedApp;
            WebApplication.InBackgroundChangeEvent += OnFocusChangedWeb;

            _volume.onClick.AddListener(ChangeSound);
        }
        
        public void Load(bool isAllowed)
        {
            _isAllowed = PlayerPrefs.HasKey(nameof(PlayerCharacteristics.IsAllowedSound))
                    ? Convert.ToBoolean(PlayerPrefs.GetString(nameof(PlayerCharacteristics.IsAllowedSound)))
                    : isAllowed;

            ChangeVolume();
        }

        public void AddMuted(AudioSource source)
        {
            _sources.Add(source);
        }

        public void Pause()
        {
            _isGamePause = true;
            Time.timeScale = (float)ValueConstants.Zero;

            foreach (var source in _sources.Where(source => source))
                source.Pause();
        }

        public void Release()
        {
            _isGamePause = false;
            Time.timeScale = (float)ValueConstants.One;

            foreach (var source in _sources.Where(source => source))
                source.UnPause();
        }
        
        public void FocusPause(bool isForce = false)
        {
            if (isForce == true)
                _isForcePause = true;
            
            Time.timeScale = (float)ValueConstants.Zero;
            AudioListener.pause = true;
        }

        public void FocusRelease(bool isForce = false)
        {
            if (isForce == true)
                _isForcePause = false;
            
            if (_isForcePause == true)
                return;
            
            AudioListener.pause = false;
            
            if (_isGamePause == true)
                return;
            
            Time.timeScale = (float)ValueConstants.One;
        }

        private void ChangeSound()
        {
            _isAllowed = !_isAllowed;
            ChangeVolume();
            SoundChanged?.Invoke(_isAllowed);
            
            PlayerPrefs.SetString(nameof(PlayerCharacteristics.IsAllowedSound), _isAllowed.ToString());
            PlayerPrefs.Save();
        }

        private void ChangeVolume()
        {
            if (_isAllowed == true)
            {
                AudioListener.volume = (float)ValueConstants.One;
                _music.UnPause();
                _icon.sprite = _onIcon;
                return;
            }
            
            AudioListener.volume = (float)ValueConstants.Zero;
            _music.Pause();
            _icon.sprite = _offIcon;
        }

        private void OnFocusChangedApp(bool isInApp)
        {
            if (isInApp == false)
                FocusPause();
            else
                FocusRelease();
        }

        private void OnFocusChangedWeb(bool isBackGround)
        {
            if (isBackGround == false)
                FocusRelease();
            else
                FocusPause();
        }
    }
}