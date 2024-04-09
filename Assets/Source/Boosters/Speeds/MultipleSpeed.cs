using UnityEngine;

namespace Boosters
{
    public class MultipleSpeed : IMovable
    {
        private readonly IMovable _movable;
        private readonly float _scaler;

        public MultipleSpeed(IMovable movable, float scaler, float lifeTime, Sprite icon)
        {
            _movable = movable;
            _scaler = scaler;
            LifeTime = lifeTime;
            Icon = icon;
        }
        
        public float LifeTime { get; }
        public Sprite Icon { get; }

        public void Accept(IBoosterVisitor visitor)
        {
            visitor.Visit(this);
        }

        public float GetSpeed()
        {
            return _movable.GetSpeed() * _scaler;
        }
    }
}