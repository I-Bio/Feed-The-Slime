using System;
using System.Collections.Generic;
using Boosters;
using Cinemachine;
using Input;
using Players;
using Spawners;
using TMPro;
using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(Revival))]
    [RequireComponent(typeof(FadeCaster))]
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Game _game;
        [SerializeField] private GameObject _spit;
        [SerializeField] private Stopper _stopper;
        [SerializeField] private WindowSwitcher _switcher;
        [SerializeField] private AutoSaveRequester _requester;

        [Space, Header(nameof(BoosterSpawner))] 
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private Booster _template;
        [SerializeField] private Transform _pointsHolder;
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private float _spawnDelay = 5f;

        [Space, Header(nameof(BoosterFactory))] 
        [SerializeField] private Sprite _speedIcon;
        [SerializeField] private Sprite _scoreIcon;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private float _minLifeTime = 4f;
        [SerializeField] private float[] _scaleValues;
        [SerializeField] private float[] _additionalValues;

        [Space, Header(nameof(FadeCaster))] 
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _castDelay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;

        [Space, Header("Themes")]
        [SerializeField] private Transform _center;
        [SerializeField] private Renderer _ground;
        [SerializeField] private Transform[] _zonePoints;
        [SerializeField] private SerializedPair<int, ThemePreparer[]>[] _zoneTemplates;
        [SerializeField] private SerializedPair<int, CenterPreparer[]>[] _centerTemplates;

        [Space, Header(nameof(CameraSetter))] 
        [SerializeField] private CameraSetter _setter;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Vector3 _gameViewPosition;

        [Space, Header(nameof(Revival))]
        [SerializeField] private GameObject _holder;
        [SerializeField] private TextMeshProUGUI _holderText;

        private IThemeFactory _factory;
        private Revival _revival;
        private FadeCaster _fadeCaster;
        private List<Contactable> _contactableObjects;
        private List<ISelectable> _selectables;

        public void Initialize(int completedLevels)
        {
            _revival = GetComponent<Revival>();
            _fadeCaster = GetComponent<FadeCaster>();

            _input.Initialize();
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _castDelay, _hitsCapacity);
            _setter.Initialize(_camera, _gameViewPosition);
            
            _factory = new ThemeFactory(_zoneTemplates, _centerTemplates, _ground, _player.GetComponent<IHidden>(),
                _player.GetComponent<IPlayerVisitor>(), completedLevels, _stopper.AddMuted);

            _contactableObjects = _factory.CreateCenter(_center, out _selectables);

            foreach (Transform zonePoint in _zonePoints)
            {
                _contactableObjects.AddRange(_factory.CreateTheme(zonePoint, out List<ISelectable> selectables));
                _selectables.AddRange(selectables);
            }
        }

        public void Launch(IReadOnlyCharacteristics characteristics, int reward, Action<float> progressChangedCallback)
        {
            IMovable movable = new Speed(characteristics.Speed);
            ICalculableScore calculableScore = new AdditionalScore(new Score(), characteristics.ScorePerEat);

            if (characteristics.DidObtainSpit)
                _spit.SetActive(true);

            _setter.ChangeToGameView();
            _revival.Initialize(_player.transform, characteristics.LifeCount, _holder, _holderText);
            _boosterSpawner.Initialize(new BoosterFactory(_scaleValues, _additionalValues,
                _speedIcon, _scoreIcon, _maxLifeTime, _minLifeTime, _pointsHolder, _offSet, _boosterSpawner.Pull<Booster>), 
                _spawnDelay, _template);
            _game.Initialize(_revival, _switcher, _stopper, _requester, reward);
            _player.Initialize(movable, calculableScore, characteristics.ProgressScore, _game, _revival,
                _contactableObjects, _selectables, progressChangedCallback);
        }
    }
}