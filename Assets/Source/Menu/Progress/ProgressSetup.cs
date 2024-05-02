using System;
using Spawners;
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
        [SerializeField] private int _advertStep;
        [SerializeField] private SerializedPair<int, int>[] _rewardSteps;
        [SerializeField] private Button _play;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _crystals;
        [SerializeField] private WindowSwitcher _switcher;
        
        [Space, Header(nameof(RewardReproducer))]
        [SerializeField] private RewardReproducer _reward;
        [SerializeField] private RewardGem _gemTemplate;

        private IProgressionBar[] _bars;

        private Progress _model;
        private ProgressPresenter _presenter;

        public void Initialize(YandexLeaderboard leaderboard, LevelBootstrap bootstrap, Stopper stopper)
        {
            _bars = new IProgressionBar[Enum.GetValues(typeof(PurchaseNames)).Length];
            _bars[(int)PurchaseNames.Speed] = _speedBar;
            _bars[(int)PurchaseNames.Score] = _scoreBar;
            _bars[(int)PurchaseNames.Life] = _lifeBar;
            _bars[(int)PurchaseNames.Spit] = _spitBar;

            _model = new Progress(_startCharacteristics, _rewardSteps, _advertStep);
            _presenter = new ProgressPresenter(_model, _bars, _play, _level, _crystals, _reward, _switcher,
                leaderboard, bootstrap, stopper, _startCharacteristics);
            _switcher.Initialize(stopper);
            _reward.Initialize(_gemTemplate);
            _presenter.Enable();
        }

        private void OnDestroy()
        {
            _presenter.Disable();
        }
    }
}