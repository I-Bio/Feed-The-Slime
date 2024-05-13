using UnityEngine;

namespace Boosters
{
    public abstract class Stat : IStat
    {
        protected readonly float Value;
        private readonly string Sign;
        
        protected Stat(float value, float lifeTime, Sprite icon, string sign)
        {
            Value = value;
            LifeTime = lifeTime;
            Icon = icon;
            Sign = sign;
        }
        
        public float LifeTime { get; }
        public Sprite Icon { get; }
        public abstract void Accept(IBoosterVisitor visitor);
        
        public override string ToString()
        {
            return $"{Sign}{Value}";
        }
    }
}