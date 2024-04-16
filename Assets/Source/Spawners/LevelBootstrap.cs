using Boosters;
using Enemies;
using Input;
using Menu;
using Players;
using UnityEngine;

namespace Spawners
{
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Game _game;

        [SerializeField] private GameObject _spit;
        
        [SerializeField] private Transform _center;
        [SerializeField] private Renderer _ground;
        [SerializeField] private Transform[] _zonePoints;
        [SerializeField] private SerializedPair<int, ThemePreparer[]>[] _zoneTemplates;
        [SerializeField] private SerializedPair<int, CenterPreparer[]>[] _centerTemplates;

        private void Awake()
        {
            ITransferService transferService = TransferService.Instance;
            IMovable movable = new Speed(transferService.Characteristics.Speed);
            int id = 0;
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), transferService.Characteristics.ScorePerEat, id, null);
            Revival revival = new Revival(_player.transform, transferService.Characteristics.LifeCount);
            
            if (transferService.Characteristics.DidObtainSpit)
                _spit.SetActive(true);
            
            _game.Initialize(transferService, revival);
            _input.Initialize(_game);
            _player.Initialize(movable, calculableScore, _game);
            _boosterSpawner.Initialize(movable, calculableScore);

            for (int i = 0; i < _zoneTemplates.Length; i++)
            {
                if (_zoneTemplates[i].Key < transferService.Characteristics.CompletedLevels) 
                    continue;
                
                id = i;
                break;
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