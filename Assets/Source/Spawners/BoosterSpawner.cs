using System.Collections;
using System.Collections.Generic;
using Boosters;
using UnityEngine;

namespace Spawners
{
    public class BoosterSpawner : ObjectPool
    {
        private readonly List<Transform> _spawnPoints = new();
        
        private Transform _pointsHolder;
        private Vector3 _offSet;
        private float _delay;

        private bool _isSpawned;
        private IFactory<IStat> _factory;

        public void Initialize(IFactory<IStat> factory, Transform pointsHolder, Vector3 offSet, float delay, Booster template)
        {
            Initialize(template);
            
            _factory = factory;
            _pointsHolder = pointsHolder;
            _offSet = offSet;
            _delay = delay;
            
            CollectPoints();
            RequestBooster();
        }

        public override void Push(SpawnableObject spawnableObject)
        {
            RequestBooster();
            base.Push(spawnableObject);
        }

        private void CollectPoints()
        {
            for (int i = 0; i < _pointsHolder.childCount; i++)
                _spawnPoints.Add(_pointsHolder.GetChild(i));
        }
        
        private void Spawn()
        {
            Vector3 position = _spawnPoints.GetRandom().position + _offSet;
            Pull<Booster>(position).Initialize(_factory.Create());
        }

        private void RequestBooster()
        {
            StartCoroutine(SpawnRoutine());
        }
        
        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(_delay);
            Spawn();
        }
    }
}