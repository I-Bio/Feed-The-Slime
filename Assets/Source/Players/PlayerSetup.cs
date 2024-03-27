using Abilities;
using Boosters;
using Cinemachine;
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
    public class PlayerSetup : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private LevelBar _levelBar;
        [SerializeField] private StageBar _stageBar;
        
        [Space, Header("Player Options")] 
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _cameraScale;

        [SerializeField] private float _scoreScaler;
        [SerializeField] private float _startMaxScore;
        [SerializeField] private int _levelsPerStage;
        [SerializeField] private SatietyStage _startStage;

        [Space, Header("Move Options")] 
        [SerializeField] private Transform _rotationPoint;

        [Space, Header("Booster Effects")] 
        [SerializeField] private float _updateDelay;
        [SerializeField] private ParticleSystem _speedEffect;
        [SerializeField] private ParticleSystem _scoreEffect;

        [Space, Header("Animation")] 
        [SerializeField] private Animator _animator;
        [SerializeField] private string _eat;
        [SerializeField] private string _idle;
        [SerializeField] private string _hide;

        private PlayerCollisionDetector _collisionDetector;
        private PlayerScanner _scanner;
        private SizeScaler _sizeScaler;
        private Mover _mover;
        private PlayerAnimation _animation;
        private AbilityCaster _abilityCaster;
        private Transform _transform;

        private Goop _model;
        private BoosterInjector _injector;
        private BoosterEjector _ejector;
        private BoosterService _service;
        private BoosterVisualizer _effectsVisualizer;

        private PlayerPresenter _playerPresenter;
        private BoosterPresenter _boosterPresenter;
        private AbilityPresenter _abilityPresenter;

        public void Initialize(IMovable movable, ICalculableScore calculableScore)
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _scanner = GetComponent<PlayerScanner>();
            _sizeScaler = GetComponent<SizeScaler>();
            _mover = GetComponent<Mover>();
            _effectsVisualizer = GetComponent<BoosterVisualizer>();
            _animation = GetComponent<PlayerAnimation>();
            _abilityCaster = GetComponent<AbilityCaster>();
            _transform = transform;

            _model = new Goop(calculableScore, _startStage, _scoreScaler, _startMaxScore, _levelsPerStage);
            _injector = new BoosterInjector();
            _ejector = new BoosterEjector(calculableScore, movable);
            _service = new BoosterService();

            _playerPresenter = new PlayerPresenter(_model, _collisionDetector, _scanner, _sizeScaler, _levelBar,
                _stageBar, _service, _animation, _abilityCaster);
            _boosterPresenter = new BoosterPresenter(_model, _mover, _injector, _ejector, _service, _effectsVisualizer);
            _abilityPresenter = new AbilityPresenter(_abilityCaster, _animation, _mover);

            _scanner.SetStage(_startStage);
            _sizeScaler.Initialize(_transform, _virtualCamera, _scaleFactor, _cameraScale);
            _mover.Initialize(movable, _rotationPoint, _transform.forward);
            _effectsVisualizer.Initialize(_updateDelay, _speedEffect, _scoreEffect);
            _animation.Initialize(_animator, _eat, _idle, _hide);
            _abilityCaster.Initialize(_transform, _startStage);
        }

        private void OnEnable()
        {
            _playerPresenter.Enable();
            _boosterPresenter.Enable();
            _abilityPresenter.Enable();
        }

        private void Start()
        {
            _model.IncreaseScore();
        }

        private void OnDisable()
        {
            _playerPresenter.Disable();
            _boosterPresenter.Disable();
            _abilityPresenter.Disable();
        }
    }
}