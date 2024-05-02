using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Menu
{
    public class Game : MonoBehaviour, IGame, ILoader
    {
        [SerializeField] private float _double = 2f;
        [SerializeField] private Button _winAdvert;
        [SerializeField] private Button _loseAdvert;
        [SerializeField] private TextMeshProUGUI[] _rewards;

        private WindowSwitcher _switcher;
        private Stopper _stopper;
        private Revival _revival;
        private int _stage;
        private float _maxStage;
        private int _rewardCount;
        
        private void OnDestroy()
        {
            _winAdvert.onClick.RemoveListener(ShowWinAdvert);
            _loseAdvert.onClick.RemoveListener(ShowLoseAdvert);
        }
        
        public void Initialize(Revival revival, WindowSwitcher switcher, Stopper stopper, int reward)
        {
            _revival = revival;
            _rewardCount = reward;
            _maxStage = Enum.GetValues(typeof(SatietyStage)).Length - 1;
            SetStage(SatietyStage.Exhaustion);
            _switcher = switcher;
            _stopper = stopper;
            
            _winAdvert.onClick.AddListener(ShowWinAdvert);
            _loseAdvert.onClick.AddListener(ShowLoseAdvert);
            
            _switcher.ChangeWindow(Windows.Play);
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = (int)stage;
            UpdateReward();
        }

        public void Win()
        {
            _switcher.ChangeWindow(Windows.Win);
        }

        public void Lose()
        {
            _switcher.ChangeWindow(Windows.Lose, _revival);
        }

        public void Load()
        {
            _stopper.Release();
            _rewardCount = Mathf.CeilToInt(_rewardCount * (_stage / _maxStage));
            PlayerPrefs.SetInt(nameof(CharacteristicConstants.Reward), _rewardCount);
            SceneManager.LoadScene((int)SceneNames.Game);
        }

        private void ShowWinAdvert()
        {
            ShowRewardAdvert(DoubleReward, null);
        }

        private void ShowLoseAdvert()
        {
            ShowRewardAdvert(Respawn, _switcher.ResumeScreen);
        }

        private void ShowRewardAdvert(Action onReward, Action onClose)
        {
#if UNITY_EDITOR
            onReward?.Invoke();
            onClose?.Invoke();
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            VideoAd.Show(_stopper.Pause, onReward, onClose);
#endif
        }

        private void Respawn()
        {
            _loseAdvert.gameObject.SetActive(false);
            _revival.Revive();
        }

        private void DoubleReward()
        {
            _winAdvert.gameObject.SetActive(false);
            _rewardCount = Mathf.CeilToInt(_rewardCount * _double);
            UpdateReward();
        }

        private void UpdateReward()
        {
            foreach (TextMeshProUGUI reward in _rewards)
                reward.SetText(Mathf.CeilToInt(_rewardCount * (_stage / _maxStage)).ToString());
        }
    }
}