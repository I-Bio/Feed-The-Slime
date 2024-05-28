using System;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(MenuReturner))]
    [RequireComponent(typeof(AutoSaveRequester))]
    public class ProgressSetup : MonoBehaviour
    {
        [SerializeField] private ProgressionBar<float> _speedBar;
        [SerializeField] private ProgressionBar<float> _scoreBar;
        [SerializeField] private ProgressionBar<int> _lifeBar;
        [SerializeField] private ProgressionBar<bool> _spitBar;
        [SerializeField] private ObjectFiller _filler;
        [SerializeField] private Button _play;
        
        [Space, Header(nameof(WindowSwitcher))]
        [SerializeField] private WindowSwitcher _switcher;
        [SerializeField] private Button _upgrade;
        [SerializeField] private Button _leader;
        [SerializeField] private Button _authorize;
        [SerializeField] private Button _pause;
        [SerializeField] private Button _resume;
        [SerializeField] private Button[] _closeButtons;

        [Space, Header(nameof(AutoSaveRequester))]
        [SerializeField] private float _delay;
        
        [Space, Header("Stats")]
        [SerializeField] private PlayerCharacteristics _startCharacteristics;
        [SerializeField] private int _advertStep;
        [SerializeField] private SerializedPair<int, int>[] _rewardSteps;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _crystals;
        
        [Space, Header(nameof(RewardReproducer))]
        [SerializeField] private RewardReproducer _reward;
        [SerializeField] private RewardGem _gemTemplate;

        [Space, Header(nameof(MenuReturner))]
        [SerializeField] private Button _open;
        [SerializeField] private Button _accept;
        [SerializeField] private Button _decline;
        
        private IProgressionBar[] _bars;
        private MenuReturner _returner;
        private AutoSaveRequester _requester;
        private Progress _model;
        private ProgressPresenter _presenter;

        private void OnDestroy()
        {
            _presenter.Disable();
        }
        
        public void Initialize(YandexLeaderboard leaderboard, LevelBootstrap bootstrap, Stopper stopper, IRewardCollector endGame)
        {
            _returner = GetComponent<MenuReturner>();
            _requester = GetComponent<AutoSaveRequester>();
            
            _bars = new IProgressionBar[Enum.GetValues(typeof(PurchaseNames)).Length];
            _bars[(int)PurchaseNames.Speed] = _speedBar;
            _bars[(int)PurchaseNames.Score] = _scoreBar;
            _bars[(int)PurchaseNames.Life] = _lifeBar;
            _bars[(int)PurchaseNames.Spit] = _spitBar;

            _model = new Progress(_startCharacteristics, _rewardSteps, _advertStep);
            _presenter = new ProgressPresenter(_model, _bars, _play, _level, _crystals, _reward, _switcher, _filler, _requester,
                leaderboard, bootstrap, stopper, endGame, _startCharacteristics);
            
            _switcher.Initialize(stopper, _upgrade, _leader, _authorize, _pause, _resume, _closeButtons);
            _returner.Initialize(_open, _accept, _decline, _switcher);
            _reward.Initialize(_gemTemplate);
            _filler.Initialize();
            _requester.Initialize(_delay);
            _presenter.Enable();
        }
    }
}