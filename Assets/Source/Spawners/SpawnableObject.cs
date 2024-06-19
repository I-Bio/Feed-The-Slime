using UnityEngine;

namespace Spawners
{
    public class SpawnableObject : MonoBehaviour
    {
        private Transform _transform;
        private GameObject _gameObject;
        private IPushable _spawner;

        public SpawnableObject Initialize(IPushable spawner)
        {
            _spawner = spawner;
            _gameObject = gameObject;
            _transform = transform;

            _gameObject.SetActive(false);
            return this;
        }

        public T Pull<T>(Vector3 position)
            where T : SpawnableObject
        {
            _transform.position = position;
            return Pull<T>();
        }

        public T Pull<T>(Transform parent)
            where T : SpawnableObject
        {
            _transform.SetParent(parent);
            return Pull<T>();
        }

        public void Push()
        {
            _spawner.Push(this);
        }

        public void SetActive(bool value)
        {
            _gameObject.SetActive(value);
        }

        private T Pull<T>()
            where T : SpawnableObject
        {
            SetActive(true);
            return this as T;
        }
    }
}