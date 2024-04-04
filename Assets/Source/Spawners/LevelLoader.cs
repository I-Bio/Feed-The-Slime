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
        [SerializeField] private Transform[] _zonePoints;
        [SerializeField] private ThemePreparer[] _zoneTemplates;

        private void Awake()
        {
            ITransferService rewardService = TransferService.Instance;
            IMovable movable = new Speed(rewardService.Characteristics.Speed);
            ICalculableScore calculableScore =
                new AdditionalScore(new Score(), rewardService.Characteristics.ScorePerEat, 0f);
            
            _game.Initialize(rewardService, _player.transform);
            _player.Initialize(movable, calculableScore, _game);
            _boosterSpawner.Initialize(movable, calculableScore);

            foreach (Transform zonePoint in _zonePoints)
                Instantiate(_zoneTemplates.GetRandom(), zonePoint).Initialize(_player.GetComponent<IHidden>());
        }
    }
}