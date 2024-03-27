using Enemies;
using Foods;
using UnityEngine;

namespace Spawners
{
    public class ThemePreparer : SpawnableObject
    {
        [SerializeField] private FoodSetup[] _foods;
        [SerializeField] private EnemySetup[] _enemies;

        public void Initialize(IHidden player)
        {
            foreach (FoodSetup food in _foods)
                food.Initialize();
            
            foreach (EnemySetup enemy in _enemies)
                enemy.Initialize(player, new NormalEnemyPolicy());
        }
    }
}