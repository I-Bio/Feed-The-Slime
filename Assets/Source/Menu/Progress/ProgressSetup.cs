using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressSetup : MonoBehaviour
    {
        [SerializeField] private ProgressionBar<float> _speedBar;
        [SerializeField] private ProgressionBar<float> _scoreBar;
        [SerializeField] private ProgressionBar<int> _lifeBar;
        [SerializeField] private ProgressionBar<bool> _spitBar;
        [SerializeField] private PlayerCharacteristics _startCharacteristics;
        [SerializeField] private SerializedPair<int>[] _rewardSteps;
        [SerializeField] private Button _play;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _crystals;
        [SerializeField] private WindowSwitcher _switcher;

        private IProgressionBar[] _bars;

        private Progress _model;
        private ProgressPresenter _presenter;

        private void Awake()
        {
            _bars = new IProgressionBar[Enum.GetValues(typeof(PurchaseNames)).Length];
            _bars[(int)PurchaseNames.Speed] = _speedBar;
            _bars[(int)PurchaseNames.Score] = _scoreBar;
            _bars[(int)PurchaseNames.Life] = _lifeBar;
            _bars[(int)PurchaseNames.Spit] = _spitBar;

            _model = new Progress(_startCharacteristics, _rewardSteps);
            _presenter = new ProgressPresenter(_model, _bars, _play, _level, _crystals);
            
            _switcher.Hide();
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif
        }

        private void OnEnable()
        {
            _presenter.Enable();
        }

        private void Start()
        {
            _model.CompleteLoad();
            
            if (TransferService.Instance.TryGetReward(out int value) == false)
                return;
            
            _model.RewardReceive(value);
        }

        private void OnDisable()
        {
            _presenter.Disable();
        }

        private void OnDestroy()
        {
            _model.Save();
        }
    }
}