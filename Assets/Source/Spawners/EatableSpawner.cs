using System;
using Foods;
using Players;
using UnityEngine;

namespace Spawners
{
    public class EatableSpawner : ObjectPool
    {
        private IPlayerVisitor _player;

        public event Action<Contactable, ISelectable> Spawned;
        
        public void Initialize(FoodSetup dissolved, IPlayerVisitor player)
        {
            Initialize(dissolved);
            _player = player;
        }
        
        public void Spawn(Vector3 position, float score)
        {
            Spawned?.Invoke(Pull<FoodSetup>(position).Initialize(score, _player, out ISelectable selectable), selectable);
        }
    }
}