using Boosters;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour, ISettable, IReader, IMover
    {
        private Rigidbody _rigidbody;
        private IMovable _movable;
        private Transform _rotationPoint;
        private Vector2 _input;
        private Vector3 _forward;
        private bool _canMove;
        private bool _didInitialize;

        private void FixedUpdate()
        {
            if (_didInitialize == false)
                return;
            
            Move();
            RotateAlongMove();
        }

        public void Initialize(IMovable movable, Transform rotationPoint, Vector3 forward)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _forward = forward;
            _movable = movable;
            _rotationPoint = rotationPoint;
            _canMove = true;
            _didInitialize = true;
        }
    
        public void SetBoost(IStatBuffer boost)
        {
            _movable = boost as IMovable;
        }

        public void ReadInput(Vector2 input)
        {
            _input = input;
        }

        public void AllowMove()
        {
            _canMove = true;
        }

        public void ProhibitMove()
        {
            _canMove = false;   
        }

        private void Move()
        {
            if (_canMove == false && _input != Vector2.zero && _rigidbody.velocity == Vector3.zero)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            if (_rigidbody.velocity == Vector3.zero && _input == Vector2.zero)
                return;
            
            _rigidbody.velocity = new Vector3(_input.x, 0f, _input.y) * _movable.GetSpeed();
        }

        private void RotateAlongMove()
        {
            if (_input == Vector2.zero)
                return;
        
            Vector3 direction = new Vector3(_input.x, 0f, _input.y);
            _rotationPoint.eulerAngles = new Vector3(0f, Vector3.SignedAngle(_forward, direction, Vector3.up), 0f);
        }
    }
}