using System;
using System.Collections.Generic;
using Boosters;
using Cinemachine;
using Foods;
using Menu;
using Spawners;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(PlayerCollisionDetector))]
    [RequireComponent(typeof(PlayerScanner))]
    [RequireComponent(typeof(SizeScaler))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(BoosterVisualizer))]
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(AbilityCaster))]
    [RequireComponent(typeof(EffectReproducer))]
    [RequireComponent(typeof(SoundReproducer))]
    [RequireComponent(typeof(Ticker))]
    [RequireComponent(typeof(EatableSpawner))]
    public class PlayerSetup : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private LevelBar _levelBar;
        [SerializeField] private StageBar _stageBar;
        [SerializeField] private ToxinBar _toxinBar;

        [Space, Header("Player Options")] 
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private LevelConfig _levelConfig;

        [Space, Header("Move Options")] 
        [SerializeField] private Transform _rotationPoint;

        [Space, Header("Sounds")] 
        [SerializeField] private AudioSource[] _sources;
        
        [Space, Header("Effects")]
        [SerializeField] private ParticleSystem[] _effects;
        
        [Space, Header("Booster Effects")] 
        [SerializeField] private float _updateDelay;
        [SerializeField] private Transform _holder;
        [SerializeField] private BoostIcon _icon;

        [Space, Header("Animation")] 
        [SerializeField] private Animator _animator;
        [SerializeField] private string _eat;
        [SerializeField] private string _idle;
        [SerializeField] private string _hide;
        
        [Space, Header("Ability")] 
        [SerializeField] private int _pointsCount;
        [SerializeField] private float _castStrength;
        [SerializeField] private Vector3 _castOffset;
        [SerializeField] private FoodSetup _dissolved;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private AbilityButton _spitButton;

        private PlayerCollisionDetector _collisionDetector;
        private PlayerScanner _scanner;
        private SizeScaler _sizeScaler;
        private Mover _mover;
        private PlayerAnimation _animation;
        private AbilityCaster _abilityCaster;
        private EffectReproducer _effectReproducer;
        private SoundReproducer _soundReproducer;
        private Ticker _ticker;
        private EatableSpawner _spawner;
        private Transform _transform;

        private Goop _model;
        private BoosterEjector _ejector;
        private BoosterService _service;
        private BoosterVisualizer _boosterVisualizer;
        private PlayerToxins _playerToxins;

        private PlayerPresenter _playerPresenter;
        private BoosterPresenter _boosterPresenter;
        private ToxinPresenter _toxinPresenter;

        public void Initialize(IMovable movable, ICalculableScore calculableScore, IGame game)
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _scanner = GetComponent<PlayerScanner>();
            _sizeScaler = GetComponent<SizeScaler>();
            _mover = GetComponent<Mover>();
            _boosterVisualizer = GetComponent<BoosterVisualizer>();
            _animation = GetComponent<PlayerAnimation>();
            _abilityCaster = GetComponent<AbilityCaster>();
            _effectReproducer = GetComponent<EffectReproducer>();
            _soundReproducer = GetComponent<SoundReproducer>();
            _ticker = GetComponent<Ticker>();
            _spawner = GetComponent<EatableSpawner>();
            _transform = transform;

            _model = new Goop(calculableScore, _levelConfig.StartStage, _levelConfig.ScoreScaler,
                _levelConfig.StartMaxScore, _levelConfig.LevelsPerStage);
            _ejector = new BoosterEjector(movable, calculableScore);
            _service = new BoosterService();
            _playerToxins = new PlayerToxins(_levelConfig.MaxToxinsCount, _levelConfig.MinToxinsCount);

            _playerPresenter = new PlayerPresenter(_model, _collisionDetector, _scanner, _sizeScaler, _levelBar,
                _stageBar, _service, _animation, _abilityCaster, _mover, _effectReproducer, _soundReproducer, game);
            _boosterPresenter = new BoosterPresenter(_model, _mover, _service, _boosterVisualizer, _ejector);
            _toxinPresenter = new ToxinPresenter(_playerToxins, _toxinBar, _ticker, _collisionDetector);
            
            _levelBar.Initialize(_levelConfig.StartScore, _levelConfig.StartMaxScore);
            _stageBar.Initialize(_levelConfig.StartMaxScore, _levelConfig.LevelsPerStage, _levelConfig.ScoreScaler);
            _scanner.SetStage(_levelConfig.StartStage);
            _sizeScaler.Initialize(_transform, _virtualCamera, _levelConfig.ScaleFactor, _levelConfig.CameraScale);
            _mover.Initialize(movable, _rotationPoint, _transform.forward);
            _boosterVisualizer.Initialize(_updateDelay, new Dictionary<Type, Action>
            {
                {typeof(IMovable), () => {_effectReproducer.PlayEffect(EffectType.SpeedBoost);}},
                {typeof(ICalculableScore), () => {_effectReproducer.PlayEffect(EffectType.ScoreBoost);}},
            }, _holder, _icon);
            _animation.Initialize(_animator, _eat, _idle, _hide);
            _abilityCaster.Initialize(_rotationPoint, _levelConfig.StartStage, _pointsCount, _castStrength, _castOffset, _spawner, _spitButton, _projectile);
            _toxinBar.Initialize(_levelConfig.MaxToxinsCount, _levelConfig.MinToxinsCount);
            _effectReproducer.Initialize(_effects);
            _soundReproducer.Initialize(_sources);
            _ticker.Initialize();
            _spawner.Initialize(_dissolved);
            _collisionDetector.SetStage(_levelConfig.StartStage);
            
            _playerPresenter.Enable();
            _boosterPresenter.Enable();
            _toxinPresenter.Enable();
        }

        private void OnDestroy()
        {
            _playerPresenter?.Disable();
            _boosterPresenter?.Disable();
            _toxinPresenter?.Disable();
        }
    }
}