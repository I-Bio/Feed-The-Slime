using Boosters;
using Cinemachine;
using Enemies;
using Foods;
using Input;
using Menu;
using Players;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
    public class GuideBootstrap : MonoBehaviour
    {
        private const float OffAlpha = 0f;
        private const float OnAlpha = 1f;

        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Guide _guide;
        [SerializeField] private ThemePreparer _theme;
        
        [Space, Header("FadeCaster")] 
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _delay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;
        
        [Space, Header("Guide")] 
        [SerializeField] private CanvasGroup _fadeBackground;
        [SerializeField] private CanvasGroup _endGame;
        [SerializeField] private CanvasGroup _mainUi;
        [SerializeField] private Button[] _nextButtons;
        [SerializeField] private Button[] _releaseButtons;
        [SerializeField] private Button _win;
        [SerializeField] private Button _pause;
        [SerializeField] private ObjectHighlighter _exampleFood;
        [SerializeField] private float _selectValue = 4f;
        [SerializeField] private Transform _enemy;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private GuideBeacon _beacon;
        [SerializeField] private GuideTrigger _trigger;
        [SerializeField] private Button _close;
        [SerializeField] private TextMeshProUGUI[] _rewards;

        private FadeCaster _fadeCaster;
        
        private void Awake()
        {
            ITransferService transferService = TransferService.Instance;
            IMovable movable = new Speed(transferService.Characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), transferService.Characteristics.ScorePerEat, OffAlpha, null);
            IHidden hidden = _player.GetComponent<IHidden>();
            EnemyDependencyVisitor visitor = new EnemyDependencyVisitor(_player.GetComponent<IPlayerVisitor>());
            _fadeCaster = GetComponent<FadeCaster>();

            _input.Initialize(_guide);
            _player.Initialize(movable, calculableScore, _guide);
            _guide.Initialize(transferService, _fadeBackground, _endGame, _mainUi, _nextButtons, _releaseButtons, _win, _pause,
                OffAlpha, OnAlpha, _rewards);
            _boosterSpawner.Initialize(movable, calculableScore);
            _theme.Initialize(hidden, visitor);
            _beacon.Initialize(_guide);
            _trigger.Initialize(_camera, _close, _player.transform, _enemy,
                () => { _guide.ChangeWindow(GuideWindows.Enemy); },
                () => { _guide.Release(); });
            _exampleFood.Initialize(OffAlpha, _selectValue);
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _delay, _hitsCapacity);

            _exampleFood.SetSelection();
        }
    }
}