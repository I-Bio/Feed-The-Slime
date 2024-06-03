﻿using DG.Tweening;
using Players;
using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(Collider))]
    public class EdiblePart : Contactable, IEatable
    {
        private const float EatingDuration = 0.08f;
        
        private float _scorePerEat;
        private bool _isAllowed;
        private bool _isEaten;
        private IPlayerVisitor _player;
        private Collider _collider;
        private Transform _transform;

        public float Score => _scorePerEat;

        public void Initialize(float scorePerEat, IPlayerVisitor player, Transform transform)
        {
            _scorePerEat = scorePerEat;
            _player = player;
            _transform = transform;
            _collider = GetComponent<Collider>();
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
        
        public void OnEat(Vector3 position)
        {
            _collider.enabled = false;
            _transform.DOMove(position, EatingDuration).OnComplete(() =>
            {
                _player.Visit(this, _scorePerEat);
                OnEatingCompletion();
            });
        }

        public virtual void OnInitialize() {}
        
        public virtual void OnEatingCompletion()
        {
            Destroy(gameObject);
        }
    }
}