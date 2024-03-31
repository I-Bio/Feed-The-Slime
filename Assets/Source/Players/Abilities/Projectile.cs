using Foods;
using Spawners;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : SpawnableObject
    {
        [SerializeField] private FoodSetup _template;

        private EatableSpawner _spawner;
        private Rigidbody _rigidbody;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out EdiblePart food))
            {
                food.Allow();
                food.TryEat(out float score);
                _spawner.Spawn(collision.transform.position, score);
            }
            
            _rigidbody.velocity = Vector3.zero;
            Push();
        }

        public void Initialize(Vector3 velocity, EatableSpawner spawner)
        {
            _spawner = spawner;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.velocity = velocity;
        }
    }
}