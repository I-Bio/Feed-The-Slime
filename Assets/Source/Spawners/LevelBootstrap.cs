using Boosters;
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
        private const float OffAlpha = 0f;
        private const float OnAlpha = 1f;
        
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Game _game;
        [SerializeField] private GameObject _spit;
        
        [Space, Header("FadeCaster")] 
        [SerializeField] private Transform _camera;
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _delay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;
        
        [Space, Header("Themes")] 
        [SerializeField] private Transform _center;
        [SerializeField] private Renderer _ground;
        [SerializeField] private Transform[] _zonePoints;
        [SerializeField] private SerializedPair<int, ThemePreparer[]>[] _zoneTemplates;
        [SerializeField] private SerializedPair<int, CenterPreparer[]>[] _centerTemplates;

        private Revival _revival;
        private FadeCaster _fadeCaster;

        private void Awake()
        {
            ITransferService transferService = TransferService.Instance;
            IMovable movable = new Speed(transferService.Characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), transferService.Characteristics.ScorePerEat, OffAlpha, null);
            _revival = GetComponent<Revival>();
            _fadeCaster = GetComponent<FadeCaster>();

            if (transferService.Characteristics.DidObtainSpit)
                _spit.SetActive(true);

            _game.Initialize(transferService, _revival, OffAlpha, OnAlpha);
            _input.Initialize(_game);
            _player.Initialize(movable, calculableScore, _game);
            _revival.Initialize(_player.transform, transferService.Characteristics.LifeCount);
            _boosterSpawner.Initialize(movable, calculableScore);
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera, _delay, _hitsCapacity);
            
            int id = 0;

            for (int i = 0; i < _zoneTemplates.Length; i++)
            {
                if (_zoneTemplates[i].Key > transferService.Characteristics.CompletedLevels)
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
    }
}