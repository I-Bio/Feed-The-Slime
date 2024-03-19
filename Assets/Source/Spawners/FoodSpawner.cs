using System.Collections.Generic;
using Foods;
using UnityEngine;

namespace Spawners
{
    public class FoodSpawner : SpawnableObject
    {
        private readonly List<Transform> _lightPoints;
        private readonly List<Transform> _middlePoints;
        private readonly List<Transform> _bigPoints;

        [SerializeField] private float _maxValue;
        [SerializeField] private float _minValue;
        [SerializeField] private float _chance;
        
        [SerializeField] private EdiblePart[] _lightTemplates;
        [SerializeField] private Transform _lightHolder;
        
        [SerializeField] private EdiblePart[] _middleTemplates;
        [SerializeField] private Transform _middleHolder;
        
        [SerializeField] private EdiblePart[] _bigTemplates;
        [SerializeField] private Transform _bigHolder;
        
        public void Initialize()
        {
            CollectPoints();
            SpawnAll();
        }

        private void CollectPoints()
        {
            for (int i = 0; i < _lightHolder.childCount; i++)
                _lightPoints.Add(_lightHolder.GetChild(i));
            
            for (int i = 0; i < _middleHolder.childCount; i++)
                _middlePoints.Add(_middleHolder.GetChild(i));
            
            for (int i = 0; i < _bigHolder.childCount; i++)
                _bigPoints.Add(_bigHolder.GetChild(i));
        }
        
        private void SpawnAll()
        {
            foreach (Transform point in _lightPoints)
            {
                if (Random.Range(_minValue, _maxValue) <= _chance)
                    return;

                Instantiate(_lightTemplates.GetRandom(), point.position, Quaternion.identity);
            }
            
            foreach (Transform point in _middlePoints)
            {
                if (Random.Range(_minValue, _maxValue) <= _chance)
                    return;

                Instantiate(_middleTemplates.GetRandom(), point.position, Quaternion.identity);
            }
            
            foreach (Transform point in _bigPoints)
            {
                if (Random.Range(_minValue, _maxValue) <= _chance)
                    return;

                Instantiate(_bigTemplates.GetRandom(), point.position, Quaternion.identity);
            }
        }
    }
}