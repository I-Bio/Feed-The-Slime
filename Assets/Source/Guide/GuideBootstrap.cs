using Boosters;
using Cinemachine;
using Enemies;
using Foods;
using Input;
using Menu;
using Players;
using Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
    public class GuideBootstrap : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerCharacteristics _characteristics;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Guide _guide;
        [SerializeField] private ThemePreparer _theme;
        
        [Space, Header("FadeCaster")] 
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _delay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;
        
        [Space, Header("Guide")]
        [SerializeField] private Button[] _nextButtons;
        [SerializeField] private Button[] _releaseButtons;
        [SerializeField] private ObjectHighlighter _exampleFood;
        [SerializeField] private float _selectValue = 4f;
        [SerializeField] private Transform _enemy;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private GuideBeacon _beacon;
        [SerializeField] private GuideTrigger _trigger;
        [SerializeField] private Button _close;
        [SerializeField] private Button _pause;

        private FadeCaster _fadeCaster;
        private SaveService _saveService;
        
        private void Awake()
        {
            _saveService = new SaveService(Launch, _characteristics);
            _saveService.Load();
        }

        private void Launch(IReadOnlyCharacteristics characteristics)
        {
            IMovable movable = new Speed(characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), characteristics.ScorePerEat, float.MinValue, null);
            IHidden hidden = _player.GetComponent<IHidden>();
            EnemyDependencyVisitor visitor = new EnemyDependencyVisitor(_player.GetComponent<IPlayerVisitor>());
            _fadeCaster = GetComponent<FadeCaster>();

            _input.Initialize(_guide);
            _player.Initialize(movable, calculableScore, _guide);
            _guide.Initialize(_nextButtons, _releaseButtons, _pause);
            _boosterSpawner.Initialize(movable, calculableScore);
            _theme.Initialize(hidden, visitor);
            _beacon.Initialize(_guide);
            _trigger.Initialize(_camera, _close, _player.transform, _enemy,
                () => { _guide.ChangeWindow(GuideWindows.Enemy); },
                () => { _guide.Release(); });
            _exampleFood.Initialize((float)CharacteristicConstants.Zero, _selectValue);
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _delay, _hitsCapacity);

            _exampleFood.SetSelection();
        }
    }
}