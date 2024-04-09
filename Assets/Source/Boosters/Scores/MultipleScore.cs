using UnityEngine;

namespace Boosters
{
    public class MultipleScore : ICalculableScore
    {
        private readonly ICalculableScore _calculable;
        private readonly float _scaler;
        
        
        public MultipleScore(ICalculableScore calculable, float scaler, float lifeTime, Sprite icon)
        {
            _calculable = calculable;
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

        public float CalculateScore(float score)
        {
            return _calculable.CalculateScore(score) * _scaler;
        }
    }
}