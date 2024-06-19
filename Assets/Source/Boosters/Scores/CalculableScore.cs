using UnityEngine;

namespace Boosters
{
    public abstract class CalculableScore : Stat, ICalculableScore
    {
        public CalculableScore(float value, float lifeTime, Sprite icon, string sign)
            : base(value, lifeTime, icon, sign)
        {
        }

        public override void Accept(IBoosterVisitor visitor)
        {
            visitor.Visit(this);
        }

        public abstract float CalculateScore(float score);
    }
}