using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<Contactable> _contactableObjects;
        private List<ISelectable> _highlighters;

        public List<Contactable> Initialize(IHidden player, IPlayerVisitor visitor, out List<ISelectable> selectables,
            Action<AudioSource> onAudioFound = null)
        {
            _contactableObjects = new();
            _highlighters = new();
            
            foreach (FoodSetup food in _foods)
            {
                _contactableObjects.Add(food.Initialize(float.NaN, visitor, out ISelectable selectable));
                _highlighters.Add(selectable);
            }
            
            foreach (EnemySetup enemy in _enemies)
            {
                _contactableObjects.AddRange(enemy.Initialize(player, visitor, out ISelectable selectable));
                _highlighters.Add(selectable);
                
                if (onAudioFound == null)
                    continue;
                
                if (enemy.TryGetComponent(out AudioSource source) == true)
                    onAudioFound(source);
            }

            selectables = _highlighters;
            return _contactableObjects;
        }
    }
}