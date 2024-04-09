using UnityEngine;

namespace Boosters
{
    public class AdditionalScore : ICalculableScore
    {
        private readonly ICalculableScore _calculable;
        private readonly float _additionValue;

        public AdditionalScore(ICalculableScore calculable, float additionValue, float lifeTime, Sprite icon)
        {
            _calculable = calculable;
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

        public float CalculateScore(float score)
        {
            return _calculable.CalculateScore(score) + _additionValue;
        }
    }
}