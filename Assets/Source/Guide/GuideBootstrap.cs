using System.Collections.Generic;
using Boosters;
using Cameras;
using Cinemachine;
using Enemies;
using Foods;
using Input;
using Menu;
using Players;
using Spawners;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Guide
{
    [RequireComponent(typeof(FadeCaster))]
    public class GuideBootstrap : MonoBehaviour
    {
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerCharacteristics _characteristics;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private ThemePreparer _theme;
        
        [Space, Header("Sound")]
        [SerializeField] private Sprite _onIcon;
        [SerializeField] private Sprite _offIcon;
        [SerializeField] private Button _volume;
        [SerializeField] private Image _icon;
        [SerializeField] private List<AudioSource> _sources;
        [SerializeField] private AudioSource _music;
        
        [Space, Header(nameof(FadeCaster))] 
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _castDelay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;
        
        [Space, Header(nameof(BoosterSpawner))]
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private Booster _template;
        [SerializeField] private Transform _pointsHolder;
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private float _spawnDelay = 5f;
        
        [Space, Header(nameof(BoosterStatFactory))]
        [SerializeField] private Sprite _speedIcon;
        [SerializeField] private Sprite _scoreIcon;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private float _minLifeTime = 4f;
        [SerializeField] private float[] _scaleValues;
        [SerializeField] private float[] _additionalValues;
        
        [Space, Header(nameof(Guide))]
        [SerializeField] private Guide _guide;
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
            ICalculableScore calculableScore = new AdditionalScore(new Score(), characteristics.ScorePerEat);
            IHidden hidden = _player.GetComponent<IHidden>();
            EnemyDependencyVisitor visitor = new EnemyDependencyVisitor(_player.GetComponent<IPlayerVisitor>());
            PlayerPrefs.SetString(nameof(PlayerCharacteristics.IsAllowedSound), characteristics.IsAllowedSound.ToString());
            
            _fadeCaster = GetComponent<FadeCaster>();

            _input.Initialize();
            _player.Initialize(movable, calculableScore, _guide);
            _boosterSpawner.Initialize(new BoosterStatFactory(_scaleValues, _additionalValues,
                _speedIcon, _scoreIcon, _maxLifeTime, _minLifeTime), _pointsHolder, _offSet, _spawnDelay, _template);
            _theme.Initialize(hidden, visitor);
            
            _beacon.Initialize(_guide);
            _trigger.Initialize(_camera, _close, _player.transform, _enemy,
                () => { _guide.ChangeWindow(GuideWindows.Enemy); },
                () => { _guide.Release(); });
            _exampleFood.Initialize((float)ValueConstants.Zero, _selectValue);
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _castDelay, _hitsCapacity);
            _filler.Initialize();
            _exampleFood.SetSelection();
            _guide.Initialize(_nextButtons, _releaseButtons, _loadButtons, _pause, _filler, _onIcon, _offIcon, _volume, _icon, _sources, _music);
        }
    }
}