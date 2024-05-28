using System;
using Boosters;
using Cinemachine;
using Enemies;
using Input;
using Players;
using Spawners;
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

        [Space, Header(nameof(BoosterStatFactory))] 
        [SerializeField] private Sprite _speedIcon;
        [SerializeField] private Sprite _scoreIcon;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private float _minLifeTime = 4f;
        [SerializeField] private float[] _scaleValues;
        [SerializeField] private float[] _additionalValues;

        [Space, Header("FadeCaster")] 
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

        private Revival _revival;
        private FadeCaster _fadeCaster;

        public void Initialize(int completedLevels)
        {
            _revival = GetComponent<Revival>();
            _fadeCaster = GetComponent<FadeCaster>();

            _input.Initialize();
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _castDelay, _hitsCapacity);
            _setter.Initialize(_camera, _gameViewPosition);

            int id = 0;

            for (int i = 0; i < _zoneTemplates.Length; i++)
            {
                if (_zoneTemplates[i].Key > completedLevels)
                    break;

                id = i;
            }

            IHidden hidden = _player.GetComponent<IHidden>();
            IPlayerVisitor visitor = _player.GetComponent<IPlayerVisitor>();

            Instantiate(_centerTemplates[id].Value.GetRandom(), _center)
                .Initialize(hidden, visitor, _ground);

            foreach (Transform zonePoint in _zonePoints)
                Instantiate(_zoneTemplates[id].Value.GetRandom(), zonePoint)
                    .Initialize(hidden, visitor, _stopper.AddMuted);
        }

        public void Launch(IReadOnlyCharacteristics characteristics, int reward, Action<float> progressChangedCallback)
        {
            IMovable movable = new Speed(characteristics.Speed);
            ICalculableScore calculableScore = new AdditionalScore(new Score(), characteristics.ScorePerEat);

            if (characteristics.DidObtainSpit)
                _spit.SetActive(true);

            _setter.ChangeToGameView();
            _revival.Initialize(_player.transform, characteristics.LifeCount);
            _boosterSpawner.Initialize(new BoosterStatFactory(_scaleValues, _additionalValues,
                _speedIcon, _scoreIcon, _maxLifeTime, _minLifeTime), _pointsHolder, _offSet, _spawnDelay, _template);
            _game.Initialize(_revival, _switcher, _stopper, _requester, reward);
            _player.Initialize(movable, calculableScore, characteristics.ProgressScore, _game, _revival, progressChangedCallback);
        }
    }
}