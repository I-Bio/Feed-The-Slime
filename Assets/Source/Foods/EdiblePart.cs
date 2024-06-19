using DG.Tweening;
using Players;
using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(Collider))]
    public class EdiblePart : Contactable, IEatable
    {
        private const float EatingDuration = 0.08f;

        private bool _isAllowed;
        private bool _isEaten;
        private IPlayerVisitor _player;
        private Collider _collider;
        private Transform _transform;

        public float Score { get; private set; }

        public void Initialize(float scorePerEat, IPlayerVisitor player, Transform transform)
        {
            Score = scorePerEat;
            _player = player;
            _transform = transform;
            _collider = GetComponent<Collider>();
            _collider.enabled = true;
            _isEaten = false;
            _isAllowed = false;
            OnInitialize();
        }

        public override bool TryContact(Bounds bounds)
        {
            if (bounds.Intersects(_collider.bounds) == false)
                return false;

            if (_isAllowed == false)
                return false;

            if (_isEaten == true)
                return false;

            _isEaten = true;
            OnEat(bounds.center);
            return true;
        }

        public void Allow()
        {
            _isAllowed = true;
        }

        public virtual void OnInitialize()
        {
        }

        public virtual void OnEatingCompletion()
        {
            Destroy(gameObject);
        }

        private void OnEat(Vector3 position)
        {
            _collider.enabled = false;
            _transform.DOMove(position, EatingDuration).OnComplete(
                () =>
                {
                    _player.Visit(this, Score);
                    OnEatingCompletion();
                });
        }
    }
}