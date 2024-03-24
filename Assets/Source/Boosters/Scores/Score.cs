namespace Boosters
{
    public class Score : ICalculableScore
    {
        public Score(float lifeTime = 0f)
        {
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
        
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