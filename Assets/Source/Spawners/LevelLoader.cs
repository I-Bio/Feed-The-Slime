using Boosters;
using Players;
using UnityEngine;

namespace Spawners
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private FoodSpawner _foodSpawner;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private Transform _playerPoint;
        [SerializeField] private float _startSpeed;
        
        private void Awake()
        {
            IMovable movable = new Speed(_startSpeed);
            ICalculableScore calculableScore = new Score();

            _foodSpawner.Initialize();
            _boosterSpawner.Initialize(movable, calculableScore);
            Instantiate(_player, _playerPoint.position, Quaternion.identity).Initialize(movable, calculableScore);
        }
    }
}