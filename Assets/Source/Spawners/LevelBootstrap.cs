using Boosters;
using Cameras;
using Cinemachine;
using Enemies;
using Input;
using Menu;
using Players;
using UnityEngine;

namespace Spawners
{
    [RequireComponent(typeof(Revival))]
    [RequireComponent(typeof(FadeCaster))]
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Game _game;
        [SerializeField] private GameObject _spit;
        
        [Space, Header("FadeCaster")] 
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _delay = 0.01f;
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
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _delay, _hitsCapacity);
            _setter.Initialize(_camera, _gameViewPosition);

            int id = 0;

            for (int i = 0; i < _zoneTemplates.Length; i++)
            {
                if (_zoneTemplates[i].Key > completedLevels)
                    break;

                id = i;
            }

            IHidden hidden = _player.GetComponent<IHidden>();
            EnemyDependencyVisitor visitor = new EnemyDependencyVisitor(_player.GetComponent<IPlayerVisitor>());

            Instantiate(_centerTemplates[id].Value.GetRandom(), _center)
                .Initialize(hidden, visitor, _ground);

            foreach (Transform zonePoint in _zonePoints)
                Instantiate(_zoneTemplates[id].Value.GetRandom(), zonePoint)
                    .Initialize(hidden, visitor);
        }

        public void Launch(IReadOnlyCharacteristics characteristics, WindowSwitcher switcher, Stopper stopper, int reward)
        {
            IMovable movable = new Speed(characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), characteristics.ScorePerEat, float.MinValue, null);

            if (characteristics.DidObtainSpit)
                _spit.SetActive(true);
            
            _setter.ChangeToGameView();
            _player.Initialize(movable, calculableScore, _game);
            _revival.Initialize(_player.transform, characteristics.LifeCount);
            _boosterSpawner.Initialize(movable, calculableScore);
            
            _game.Initialize(_revival, switcher, stopper, reward);
        }
    }
}