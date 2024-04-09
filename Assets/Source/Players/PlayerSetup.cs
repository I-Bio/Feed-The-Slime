using Boosters;
using Cinemachine;
using Menu;
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
    public class PlayerSetup : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private LevelBar _levelBar;
        [SerializeField] private StageBar _stageBar;

        [Space, Header("Player Options")] 
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [SerializeField] private LevelConfig _levelConfig;

        [Space, Header("Move Options")] 
        [SerializeField] private Transform _rotationPoint;

        [Space, Header("Booster Effects")] 
        [SerializeField] private float _updateDelay;

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
        private EffectReproducer _effectReproducer;
        private SoundReproducer _soundReproducer;
        private Transform _transform;

        private Goop _model;
        private BoosterEjector _ejector;
        private BoosterService _service;
        private BoosterVisualizer _boosterVisualizer;

        private PlayerPresenter _playerPresenter;
        private BoosterPresenter _boosterPresenter;

        public void Initialize(IMovable movable, ICalculableScore calculableScore, Game game)
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
            _transform = transform;

            _model = new Goop(calculableScore, _levelConfig.StartStage, _levelConfig.ScoreScaler,
                _levelConfig.StartMaxScore, _levelConfig.LevelsPerStage);
            _ejector = new BoosterEjector(movable, calculableScore);
            _service = new BoosterService();

            _playerPresenter = new PlayerPresenter(_model, _collisionDetector, _scanner, _sizeScaler, _levelBar,
                _stageBar, _service, _animation, _abilityCaster, _mover, _effectReproducer, _soundReproducer, game);
            _boosterPresenter = new BoosterPresenter(_model, _mover, _service, _boosterVisualizer, _ejector);

            _levelBar.Initialize(_levelConfig.StartScore, _levelConfig.StartMaxScore);
            _stageBar.Initialize(_levelConfig.StartMaxScore, _levelConfig.LevelsPerStage, _levelConfig.ScoreScaler);
            _scanner.SetStage(_levelConfig.StartStage);
            _sizeScaler.Initialize(_transform, _virtualCamera, _levelConfig.ScaleFactor, _levelConfig.CameraScale);
            _mover.Initialize(movable, _rotationPoint, _transform.forward);
            _boosterVisualizer.Initialize(_updateDelay, _effectReproducer);
            _animation.Initialize(_animator, _eat, _idle, _hide);
            _abilityCaster.Initialize(_rotationPoint, _levelConfig.StartStage);
        }

        private void OnEnable()
        {
            _playerPresenter.Enable();
            _boosterPresenter.Enable();
        }

        private void OnDisable()
        {
            _playerPresenter.Disable();
            _boosterPresenter.Disable();
        }
    }
}