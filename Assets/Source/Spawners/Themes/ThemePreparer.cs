using System;
using Enemies;
using Foods;
using Players;
using UnityEngine;

namespace Spawners
{
    public class ThemePreparer : SpawnableObject
    {
        [SerializeField] private FoodSetup[] _foods;
        [SerializeField] private EnemyPolicies _behaviour;
        [SerializeField] private EnemySetup[] _enemies;

        public void Initialize(IHidden player, EnemyDependencyVisitor visitor, Action<AudioSource> onAudioFound = null)
        {
            foreach (FoodSetup food in _foods)
                food.Initialize(float.NaN);
            
            foreach (EnemySetup enemy in _enemies)
            {
                enemy.Initialize(player, visitor, CreatePolicy());
                
                if (onAudioFound == null)
                    continue;
                
                if (enemy.TryGetComponent(out AudioSource source) == true)
                    onAudioFound?.Invoke(source);
            }
        }

        private IEnemyPolicy CreatePolicy()
        {
            return _behaviour switch
            {
                EnemyPolicies.Normal => new NormalEnemyPolicy(),
                EnemyPolicies.Smart => new SmartEnemyPolicy(),
                _ => throw new ArgumentOutOfRangeException(nameof(_behaviour), _behaviour, null)
            };
        }
    }
}