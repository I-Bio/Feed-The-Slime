namespace Boosters
{
    public class MultipleScore : ICalculableScore
    {
        private readonly ICalculableScore _calculable;
        private readonly float _scaler;
        
        
        public MultipleScore(ICalculableScore calculable, float scaler, float lifeTime)
        {
            _calculable = calculable;
            _scaler = scaler;
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
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