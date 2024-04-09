using System.Collections;
using System.Collections.Generic;
using Boosters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class BoosterSpawner : ObjectPool
    {
        private readonly List<Transform> _spawnPoints = new();
        
        [SerializeField] private Transform _pointsHolder;
        [SerializeField] private Sprite _speedIcon;
        [SerializeField] private Sprite _scoreIcon;
        [SerializeField] private Vector3 _offSet;
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
            _fabric = new BoosterFabric(movable, calculableScore, _scaleValues, _additionalValues, _speedIcon, _scoreIcon);
            
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
            BoosterType type = _boosterTypes.GetRandom();
            float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);

            Pull<Booster>(position).Initialize(_fabric.CreateBoost(type, lifeTime));
        }

        private void RequestBooster()
        {
            StartCoroutine(SpawnRoutine());
        }
        
        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(_waitTime);
            Spawn();
        }
    }
}