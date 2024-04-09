using UnityEngine;

namespace Boosters
{
    public class Speed : IMovable
    {
        private readonly float _value;

        public Speed(float value, float lifeTime = 0f, Sprite icon = null)
        {
            _value = value;
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
            return _value;
        }
    }
}