using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class ObjectPool : MonoBehaviour, IPushable
    {
        private readonly Queue<SpawnableObject> SpawnQueue = new ();

        private SpawnableObject _spawnableObject;

        public virtual void Push(SpawnableObject spawnableObject)
        {
            PushOnInitialize(spawnableObject);
        }

        public void Initialize(SpawnableObject spawnableObject)
        {
            _spawnableObject = spawnableObject;
        }

        public T Pull<T>(Vector3 position)
            where T : SpawnableObject
        {
            if (SpawnQueue.Count == 0)
                PushOnInitialize(Instantiate(_spawnableObject, position, Quaternion.identity).Initialize(this));

            return SpawnQueue.Dequeue().Pull<T>(position);
        }

        public T Pull<T>(Transform parent)
            where T : SpawnableObject
        {
            if (SpawnQueue.Count == 0)
                PushOnInitialize(Instantiate(_spawnableObject, parent).Initialize(this));

            return SpawnQueue.Dequeue().Pull<T>(parent);
        }

        private void PushOnInitialize(SpawnableObject spawnableObject)
        {
            spawnableObject.SetActive(false);
            SpawnQueue.Enqueue(spawnableObject);
        }
    }
}