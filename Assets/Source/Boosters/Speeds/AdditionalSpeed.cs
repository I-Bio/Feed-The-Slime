using UnityEngine;

namespace Boosters
{
    public class AdditionalSpeed : IMovable
    {
        private readonly IMovable _movable;
        private readonly float _additionValue;

        public AdditionalSpeed(IMovable movable, float additionValue, float lifeTime, Sprite icon)
        {
            _movable = movable;
            _additionValue = additionValue;
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
            return _movable.GetSpeed() + _additionValue;
        }
    }
}