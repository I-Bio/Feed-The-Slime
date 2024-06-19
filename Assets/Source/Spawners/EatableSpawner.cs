using System;
using Foods;
using Players;
using UnityEngine;

namespace Spawners
{
    public class EatableSpawner : ObjectPool, IEatableFactory
    {
        private IPlayerVisitor _player;

        public event Action<Contactable, ISelectable> Spawned;

        public void Initialize(FoodSetup dissolved, IPlayerVisitor player)
        {
            Initialize(dissolved);
            _player = player;
        }

        public void Create(Vector3 contactPosition, EdiblePart food)
        {
            float score = food.Score;
            food.OnEatingCompletion();
            Vector3 position = new Vector3(contactPosition.x, (float)ValueConstants.Zero, contactPosition.z);
            Spawned?.Invoke(
                Pull<FoodSetup>(position).Initialize(score, _player, out ISelectable selectable),
                selectable);
        }
    }
}