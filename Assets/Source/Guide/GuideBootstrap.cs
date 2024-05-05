using Boosters;
using Cinemachine;
using Enemies;
using Foods;
using Input;
using Menu;
using Players;
using Spawners;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Guide
{
    [RequireComponent(typeof(FadeCaster))]
    [RequireComponent(typeof(SoundChanger))]
    public class GuideBootstrap : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerCharacteristics _characteristics;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Guide _guide;
        [SerializeField] private ThemePreparer _theme;
        
        [Space, Header(nameof(SoundChanger))]
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _game;
        [SerializeField] private Slider _music;
        [SerializeField] private AudioSource _sound;
        
        [Space, Header(nameof(FadeCaster))] 
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _delay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;
        
        [Space, Header(nameof(Guide))]
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
        [SerializeField] private Button[] _loadButtons;
        [SerializeField] private ObjectFiller _filler;

        [Space, Header(nameof(SubWindowSwitcher))] 
        [SerializeField] private SubWindowSwitcher _subSwitcher;
        [SerializeField] private Button[] _subCloseButtons;
        [SerializeField] private Button[] _subVolumeButtons;
        [SerializeField] private Window[] _subParents;

        private FadeCaster _fadeCaster;
        private SoundChanger _changer;
        private GuideSaveService _saveService;
        
        private void Awake()
        {
            _saveService = new GuideSaveService(Launch, _characteristics);
            _saveService.Load();
            _changer.GameVolumeChanged += _saveService.OnGameVolumeChanged;
            _changer.MusicVolumeChanged += _saveService.OnMusicVolumeChanged;
        }

        private void OnDestroy()
        {
            _changer.GameVolumeChanged -= _saveService.OnGameVolumeChanged;
            _changer.MusicVolumeChanged -= _saveService.OnMusicVolumeChanged;
        }

        private void Launch(IReadOnlyCharacteristics characteristics)
        {
            IMovable movable = new Speed(characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), characteristics.ScorePerEat, float.MinValue, null);
            IHidden hidden = _player.GetComponent<IHidden>();
            EnemyDependencyVisitor visitor = new EnemyDependencyVisitor(_player.GetComponent<IPlayerVisitor>());
            
            _fadeCaster = GetComponent<FadeCaster>();
            _changer = GetComponent<SoundChanger>();

            _input.Initialize();
            _player.Initialize(movable, calculableScore, _guide);
            _boosterSpawner.Initialize(movable, calculableScore);
            _theme.Initialize(hidden, visitor);
            _changer.Initialize(_mixer, _game, _music, _sound);
            _beacon.Initialize(_guide);
            _trigger.Initialize(_camera, _close, _player.transform, _enemy,
                () => { _guide.ChangeWindow(GuideWindows.Enemy); },
                () => { _guide.Release(); });
            _exampleFood.Initialize((float)ValueConstants.Zero, _selectValue);
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _delay, _hitsCapacity);
            _filler.Initialize();
            _exampleFood.SetSelection();
            _changer.Load((float)ValueConstants.Zero, (float)ValueConstants.Zero);
            _guide.Initialize(_nextButtons, _releaseButtons, _loadButtons, _pause, _filler,
                _subSwitcher, _subCloseButtons, _subVolumeButtons, _subParents);
        }
    }
}