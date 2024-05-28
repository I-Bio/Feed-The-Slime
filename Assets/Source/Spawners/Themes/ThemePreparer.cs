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
        [SerializeField] private EnemySetup[] _enemies;

        public void Initialize(IHidden player, IPlayerVisitor visitor, Action<AudioSource> onAudioFound = null)
        {
            foreach (FoodSetup food in _foods)
                food.Initialize(float.NaN);
            
            foreach (EnemySetup enemy in _enemies)
            {
                enemy.Initialize(player, visitor);
                
                if (onAudioFound == null)
                    continue;
                
                if (enemy.TryGetComponent(out AudioSource source) == true)
                    onAudioFound(source);
            }
        }
    }
}