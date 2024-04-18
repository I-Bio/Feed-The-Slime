using Foods;
using UnityEngine;

namespace Spawners
{
    public class EatableSpawner : ObjectPool
    {
        public void Initialize(FoodSetup dissolved)
        {
            Initialize(dissolved as SpawnableObject);
        }
        
        public void Spawn(Vector3 position, float score)
        {
            Pull<FoodSetup>(position).Initialize(score);
        }
    }
}