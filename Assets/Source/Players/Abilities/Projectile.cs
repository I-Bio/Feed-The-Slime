using System;
using Spawners;
using UnityEngine;

namespace Abilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : SpawnableObject
    {
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rigidbody.velocity = Vector3.zero;
            Push();
        }

        public void Initialize(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
        }
    }
}