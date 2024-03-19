﻿namespace Boosters
{
    public class Score : ICalculableScore
    {
        public Score(float lifeTime)
        {
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }

        public float CalculateScore(float score)
        {
            return score;
        }
    }
}