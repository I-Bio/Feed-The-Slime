using Foods;
using Spawners;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : SpawnableObject
    {
        private IEatableFactory _factory;
        private Rigidbody _rigidbody;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out EdiblePart food))
                _factory.Create(collision.transform.position, food);

            _rigidbody.velocity = Vector3.zero;
            Push();
        }

        public Projectile Initialize(Vector3 velocity, IEatableFactory factory)
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            
            _factory = factory;
            _rigidbody.velocity = velocity;
            return this;
        }
    }
}