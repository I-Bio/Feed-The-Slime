using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(PlayerCollisionDetector))]
    [RequireComponent(typeof(PlayerScanner))]
    [RequireComponent(typeof(SizeScaler))]
    public class PlayerSetup : MonoBehaviour
    {
        [SerializeField] private LevelBar _levelBar;
        [SerializeField] private StageBar _stageBar;
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _scoreScaler;
        [SerializeField] private float _startMaxScore;
        [SerializeField] private int _levelsPerStage;
        [SerializeField] private SatietyStage _startStage;
        
        private PlayerCollisionDetector _collisionDetector;
        private PlayerScanner _scanner;
        private SizeScaler _sizeScaler;
        private Transform _transform;

        private Goop _model;
        private PlayerPresenter _presenter;
        
        private void Awake()
        {
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _scanner = GetComponent<PlayerScanner>();
            _sizeScaler = GetComponent<SizeScaler>();
            _transform = transform;

            _model = new Goop(_startStage, _scoreScaler, _startMaxScore, _levelsPerStage);
            _presenter = new PlayerPresenter(_model, _collisionDetector, _scanner, _sizeScaler, _levelBar, _stageBar);
            
            _scanner.SetSatiety(_startStage);
            _sizeScaler.Initialize(_transform, _scaleFactor);
        }

        private void OnEnable()
        {
            _presenter.Enable();
        }

        private void OnDisable()
        {
            _presenter.Disable();
        }
    }
}