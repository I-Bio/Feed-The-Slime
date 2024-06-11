using UnityEngine;

namespace Boosters
{
    public abstract class Movable : Stat, IMovable
    {
        public Movable(float value, float lifeTime, Sprite icon, string sign)
            : base(value, lifeTime, icon, sign) {}
        
        public override void Accept(IBoosterVisitor visitor)
        {
            visitor.Visit(this);
        }
        
        public abstract float GetSpeed();
    }
}