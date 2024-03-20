using Foods;
using UnityEngine;

namespace Spawners
{
    public class FoodSpawner : SpawnableObject
    {
        [SerializeField] private float _maxValue;
        [SerializeField] private float _minValue;
        [SerializeField] private float _chance;
        
        [SerializeField] private FoodSetup[] _foods;

        public void Initialize()
        {
            SpawnAll();
        }

        private void SpawnAll()
        {
            foreach (FoodSetup food in _foods)
            {
                food.Initialize();
                
                if (Random.Range(_minValue, _maxValue) <= _chance)
                    food.gameObject.SetActive(false);
            }
        }
    }
}