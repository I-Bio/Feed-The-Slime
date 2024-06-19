using UnityEngine;

namespace Boosters
{
    public abstract class Stat : IStat
    {
        private readonly string Sign;

        public Stat(float value, float lifeTime, Sprite icon, string sign)
        {
            Value = value;
            LifeTime = lifeTime;
            Icon = icon;
            Sign = sign;
        }

        public float Value { get; }

        public float LifeTime { get; }

        public Sprite Icon { get; }

        public abstract void Accept(IBoosterVisitor visitor);

        public override string ToString()
        {
            return $"{Sign}{Value}";
        }
    }
}