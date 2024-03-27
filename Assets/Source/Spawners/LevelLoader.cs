using Boosters;
using Players;
using UnityEngine;

namespace Spawners
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private float _startSpeed;
        [SerializeField] private Transform[] _zonePoints;
        [SerializeField] private ThemePreparer[] _zoneTemplates;

        private void Awake()
        {
            IMovable movable = new Speed(_startSpeed);
            ICalculableScore calculableScore = new Score();

            _player.Initialize(movable, calculableScore);
            _boosterSpawner.Initialize(movable, calculableScore);
            
            foreach (Transform zonePoint in _zonePoints)
                Instantiate(_zoneTemplates.GetRandom(), zonePoint).Initialize(_player.GetComponent<IHidden>());
        }
    }
}