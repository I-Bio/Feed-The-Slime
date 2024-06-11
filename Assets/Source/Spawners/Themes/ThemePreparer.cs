using System;
using System.Collections.Generic;
using Enemies;
using Foods;
using Players;
using UnityEngine;

namespace Spawners
{
    public class ThemePreparer : SpawnableObject
    {
        private readonly List<Contactable> ContactableObjects = new();
        private readonly List<ISelectable> Highlighters = new();
        
        [SerializeField] private FoodSetup[] _foods;
        [SerializeField] private EnemySetup[] _enemies;

        public List<Contactable> Initialize(IHidden player, IPlayerVisitor visitor, out List<ISelectable> selectables,
            Action<AudioSource> onAudioFoundCallback = null)
        {
            foreach (FoodSetup food in _foods)
            {
                ContactableObjects.Add(food.Initialize(float.NaN, visitor, out ISelectable selectable));
                Highlighters.Add(selectable);
            }
            
            foreach (EnemySetup enemy in _enemies)
            {
                ContactableObjects.AddRange(enemy.Initialize(player, visitor, out ISelectable selectable));
                Highlighters.Add(selectable);
                
                if (onAudioFoundCallback == null)
                    continue;
                
                if (enemy.TryGetComponent(out AudioSource source) == true)
                    onAudioFoundCallback(source);
            }

            selectables = Highlighters;
            return ContactableObjects;
        }
    }
}