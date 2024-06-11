using System.Collections;
using Boosters;
using UnityEngine;

namespace Spawners
{
    public class BoosterSpawner : ObjectPool
    {
        private IFactory<Booster> _factory;
        private WaitForSeconds _wait;
        private bool _isSpawned;

        public void Initialize(IFactory<Booster> factory, float delay, Booster template)
        {
            Initialize(template);
            _factory = factory;
            _wait = new WaitForSeconds(delay);
            RequestBooster();
        }

        public override void Push(SpawnableObject spawnableObject)
        {
            RequestBooster();
            base.Push(spawnableObject);
        }

        private void Spawn()
        {
            _factory.Create();
        }

        private void RequestBooster()
        {
            StartCoroutine(SpawnRoutine());
        }
        
        private IEnumerator SpawnRoutine()
        {
            yield return _wait;
            Spawn();
        }
    }
}