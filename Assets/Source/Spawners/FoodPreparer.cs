using Foods;
using UnityEngine;

namespace Spawners
{
    public class FoodPreparer : SpawnableObject
    {
        [SerializeField] private FoodSetup[] _foods;

        public void Initialize()
        {
            foreach (FoodSetup food in _foods)
                food.Initialize();
        }
    }
}