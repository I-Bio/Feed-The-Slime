using UnityEngine;

namespace Boosters
{
    public class Score : CalculableScore
    {
        public Score(float value = 0f, float lifeTime = 0f, Sprite icon = null, string sign = "") : base(value, lifeTime, icon, sign) {}
        
        public override float CalculateScore(float score)
        {
            return score;
        }
    }
}