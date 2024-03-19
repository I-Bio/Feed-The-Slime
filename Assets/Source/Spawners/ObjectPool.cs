using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class ObjectPool : MonoBehaviour, IPushable
    {
        private readonly Queue<SpawnableObject> _spawnQueue = new Queue<SpawnableObject>();
        
        [SerializeField] private SpawnableObject _spawnableObject;

        public T Pull<T>(Vector3 position) where T: class
        {
            if (_spawnQueue.Count == 0)
                Instantiate(_spawnableObject, position, Quaternion.identity).Initialize(this).Push();
            
            return _spawnQueue.Dequeue().Pull<T>(position);
        }

        public virtual void Push(SpawnableObject spawnableObject)
        {
            spawnableObject.SetActive(false);
            _spawnQueue.Enqueue(spawnableObject);
        }
    }
}