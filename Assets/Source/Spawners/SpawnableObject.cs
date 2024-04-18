using UnityEngine;

namespace Spawners
{
    public class SpawnableObject : MonoBehaviour
    {
        private GameObject _gameObject;
        private Transform _transform;
        private IPushable _spawner;

        public SpawnableObject Initialize(IPushable spawner)
        {
            _spawner = spawner;
            _gameObject = gameObject;
            _transform = transform;

            _gameObject.SetActive(false);
            return this;
        }

        public T Pull<T>(Vector3 position) where T: SpawnableObject
        {
            SetActive(true);
            _transform.position = position;
            return this as T;
        }
        
        public T Pull<T>() where T: SpawnableObject
        {
            SetActive(true);
            return this as T;
        }

        public void Push()
        {
            _spawner.Push(this);
        }

        public void SetActive(bool value)
        {
            _gameObject.SetActive(value);
        }
    }
}