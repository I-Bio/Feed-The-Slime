using System;
using Enemies;
using Foods;
using UnityEngine;

namespace Spawners
{
    public class ThemePreparer : SpawnableObject
    {
        [SerializeField] private FoodSetup[] _foods;
        [SerializeField] private EnemyBehaviours _behaviour;
        [SerializeField] private EnemySetup[] _enemies;

        public virtual void Initialize(IHidden player)
        {
            foreach (FoodSetup food in _foods)
                food.Initialize(float.NaN);
            
            foreach (EnemySetup enemy in _enemies)
                enemy.Initialize(player, CreatePolicy());
        }

        private IEnemyPolicy CreatePolicy()
        {
            return _behaviour switch
            {
                EnemyBehaviours.Normal => new NormalEnemyPolicy(),
                EnemyBehaviours.Smart => new SmartEnemyPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(_behaviour), _behaviour, null)
            };
        }
    }
}