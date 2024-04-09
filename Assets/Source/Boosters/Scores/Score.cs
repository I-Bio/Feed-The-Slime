using UnityEngine;

namespace Boosters
{
    public class Score : ICalculableScore
    {
        public Score(float lifeTime = 0f, Sprite icon = null)
        {
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
            return score;
        }
    }
}