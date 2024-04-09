using Boosters;
using Menu;
using Players;
using UnityEngine;

namespace Spawners
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Game _game;
        [SerializeField] private Transform _center;
        [SerializeField] private Renderer _ground;
        [SerializeField] private Transform[] _zonePoints;
        [SerializeField] private SerializedPair<int, ThemePreparer[]>[] _zoneTemplates;
        [SerializeField] private SerializedPair<int, CenterPreparer[]>[] _centerTemplates;

        private void Awake()
        {
            ITransferService transferService = TransferService.Instance;
            IMovable movable = new Speed(transferService.Characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), transferService.Characteristics.ScorePerEat, 0f, null);
            int id = 0;

            _game.Initialize(transferService, _player.transform);
            _player.Initialize(movable, calculableScore, _game);
            _boosterSpawner.Initialize(movable, calculableScore);

            for (int i = 0; i < _zoneTemplates.Length; i++)
            {
                if (_zoneTemplates[i].Key < transferService.Characteristics.CompletedLevels) 
                    continue;
                
                id = i;
                break;
            }
            
            Instantiate(_centerTemplates[id].Value.GetRandom(), _center)
                .Initialize(_player.GetComponent<IHidden>(), _ground);

            foreach (Transform zonePoint in _zonePoints)
                Instantiate(_zoneTemplates[id].Value.GetRandom(), zonePoint)
                    .Initialize(_player.GetComponent<IHidden>());
        }
    }
}