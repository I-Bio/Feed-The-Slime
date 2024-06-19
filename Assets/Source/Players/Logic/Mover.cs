using Boosters;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour, ISettable<IMovable>, IReader, IMover
    {
        private Rigidbody _rigidbody;
        private IMovable _movable;
        private StatCombiner<IMovable> _combiner;
        private IMovableFactory _factory;
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

        public void Initialize(IMovable movable, IMovableFactory factory, Transform rotationPoint, Vector3 forward)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _forward = forward;
            _factory = factory;
            _combiner = new StatCombiner<IMovable>(new CombinedSpeed());
            _combiner.Add(movable);
            _movable = _combiner.GetRecombined();
            _rotationPoint = rotationPoint;
            _canMove = true;
            _didInitialize = true;
        }

        public void SetBoost(IMovable boost)
        {
            _combiner.ChangeBoost(boost);
            _movable = _combiner.GetRecombined();
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

        public void Scale(SatietyStage stage)
        {
            if (stage == SatietyStage.Exhaustion)
                return;

            _combiner.AddAfterFirst(_factory.Create(stage));
            _movable = _combiner.GetRecombined();
        }

        private void Move()
        {
            if (_canMove == false)
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

            Vector3 direction = new Vector3(_input.x, (float)ValueConstants.Zero, _input.y);
            _rotationPoint.eulerAngles = new Vector3(
                (float)ValueConstants.Zero,
                Vector3.SignedAngle(_forward, direction, Vector3.up),
                (float)ValueConstants.Zero);
        }
    }
}