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

        [Space] [Header("Player Options")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private LevelConfig _levelConfig;

        [Space] [Header("Move Options")]
        [SerializeField] private Transform _rotationPoint;

        [Space] [Header("Sounds")]
        [SerializeField] private AudioSource[] _sources;

        [Space] [Header("Effects")]
        [SerializeField] private ParticleSystem[] _effects;

        [Space] [Header("Booster Effects")]
        [SerializeField] private float _updateDelay;
        [SerializeField] private Transform _holder;
        [SerializeField] private BoostIcon _icon;

        [Space] [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _eat;
        [SerializeField] private string _idle;
        [SerializeField] private string _hide;

        [Space] [Header("Scanner")]
        [SerializeField] private float _distance = 5f;

        [Space] [Header("Detector")]
        [SerializeField] private Collider _collider;

        [Space] [Header("Ability")]
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

        private Player _model;
        private BoosterService _service;
        private BoosterVisualizer _boosterVisualizer;
        private PlayerToxins _playerToxins;

        private PlayerPresenter _playerPresenter;
        private BoosterPresenter _boosterPresenter;
        private ToxinPresenter _toxinPresenter;

        private void OnDestroy()
        {
            _playerPresenter?.Disable();
            _boosterPresenter?.Disable();
            _toxinPresenter?.Disable();
        }

        public void Initialize(
            IMovable movable,
            ICalculableScore calculableScore,
            float startScore,
            IGame game,
            IRevival revival,
            List<Contactable> contactableObjects,
            List<ISelectable> selectables,
            Action<float> progressChangedCallback)
        {
            PrepareComponents();
            InitializePlayer(
                movable,
                calculableScore,
                startScore,
                game,
                revival,
                contactableObjects,
                selectables,
                progressChangedCallback);
            InitializeBoosters();
            InitializeToxins(revival);

            _playerPresenter.Enable();
            _boosterPresenter.Enable();
            _toxinPresenter.Enable();
        }

        private void PrepareComponents()
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
        }

        private void InitializePlayer(
            IMovable movable,
            ICalculableScore calculableScore,
            float startScore,
            IGame game,
            IRevival revival,
            List<Contactable> contactableObjects,
            List<ISelectable> selectables,
            Action<float> progressChangedCallback)
        {
            startScore = startScore > (float)ValueConstants.Zero ? startScore : _levelConfig.StartScore;

            _model = new Player(
                calculableScore,
                _levelConfig.StartStage,
                _levelConfig.ScoreScaler,
                startScore,
                _levelConfig.StartMaxScore,
                _levelConfig.LevelsPerStage);
            _service = new BoosterService();
            _playerPresenter = new PlayerPresenter(
                _model,
                _collisionDetector,
                _scanner,
                _sizeScaler,
                _levelBar,
                _stageBar,
                _service,
                _animation,
                _abilityCaster,
                _mover,
                _effectReproducer,
                _soundReproducer,
                _spawner,
                game,
                revival,
                progressChangedCallback);

            _levelBar.Initialize(_levelConfig.StartScore, _levelConfig.StartMaxScore);
            _stageBar.Initialize(_levelConfig.StartMaxScore, _levelConfig.LevelsPerStage, _levelConfig.ScoreScaler);
            _sizeScaler.Initialize(_transform, _virtualCamera, _levelConfig.ScaleFactor, _levelConfig.CameraScale);
            _mover.Initialize(
                movable,
                new MoverScalerFactory(movable, _levelConfig.CameraScale),
                _rotationPoint,
                _transform.forward);
            _animation.Initialize(_animator, _eat, _idle, _hide);

            _abilityCaster.Initialize(
                _transform,
                _levelConfig.StartStage,
                _pointsCount,
                _castStrength,
                _castOffset,
                new ProjectileFactory(
                    _castStrength,
                    _castOffset,
                    _transform,
                    _spawner,
                    _abilityCaster.Pull<Projectile>),
                _spitButton,
                _projectile);

            _effectReproducer.Initialize(_effects);
            _soundReproducer.Initialize(_sources);
            _spawner.Initialize(_dissolved, _collisionDetector);
            _scanner.Initialize(selectables, _levelConfig.StartStage, _transform, _distance, _levelConfig.ScaleFactor);
            _collisionDetector.Initialize(contactableObjects, _levelConfig.StartStage, _collider);
        }

        private void InitializeBoosters()
        {
            _boosterPresenter = new BoosterPresenter(_model, _mover, _service, _boosterVisualizer);

            _boosterVisualizer.Initialize(
                _updateDelay,
                _icon,
                new IconFactory(_holder, _boosterVisualizer.Pull<BoostIcon>),
                new Dictionary<Type, Action>
                {
                    { typeof(IMovable), () => _effectReproducer.Play(EffectType.SpeedBoost) },
                    { typeof(ICalculableScore), () => _effectReproducer.Play(EffectType.ScoreBoost) },
                });
        }

        private void InitializeToxins(IRevival revival)
        {
            _playerToxins = new PlayerToxins(_levelConfig.MaxToxinsCount, _levelConfig.MinToxinsCount);
            _toxinPresenter = new ToxinPresenter(_playerToxins, _toxinBar, _ticker, _collisionDetector, revival);

            _toxinBar.Initialize(_levelConfig.MaxToxinsCount, _levelConfig.MinToxinsCount);
            _ticker.Initialize();
        }
    }
}