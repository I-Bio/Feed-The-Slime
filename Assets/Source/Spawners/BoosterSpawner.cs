using System.Collections;
using System.Collections.Generic;
using Boosters;
using UnityEngine;

namespace Spawners
{
    public class BoosterSpawner : ObjectPool
    {
        private readonly List<Transform> _spawnPoints = new List<Transform>();
        
        [SerializeField] private Transform _pointsHolder;
        [SerializeField] private float _waitTime;
        [SerializeField] private float _maxLifeTime;
        [SerializeField] private float _minLifeTime;
        [SerializeField] private BoosterType[] _boosterTypes;
        [SerializeField] private float[] _scaleValues;
        [SerializeField] private float[] _additionalValues;

        private bool _isSpawned;
        private BoosterFabric _fabric;

        public void Initialize(IMovable movable, ICalculableScore calculableScore)
        {
            _fabric = new BoosterFabric(movable, calculableScore, _scaleValues, _additionalValues);
            
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
            Vector3 position = _spawnPoints.GetRandom().position;
            BoosterType type = _boosterTypes.GetRandom();
            float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
            IStatBuffer boost = _fabric.CreateBoost(type, lifeTime);
            
            Pull<Booster>(position).Initialize(boost);
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(_waitTime);
            Spawn();
        }
        
        private void RequestBooster()
        {
            StartCoroutine(SpawnRoutine());
        }
    }
}