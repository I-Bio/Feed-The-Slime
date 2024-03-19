namespace Boosters
{
    public class AdditionalScore : ICalculableScore
    {
        private readonly ICalculableScore _calculable;
        private readonly float _additionValue;

        public AdditionalScore(ICalculableScore calculable, float additionValue, float lifeTime)
        {
            _calculable = calculable;
            _additionValue = additionValue;
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
        
        public float CalculateScore(float score)
        {
            return _calculable.CalculateScore(score) + _additionValue;
        }
    }
}