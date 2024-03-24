namespace Boosters
{
    public class Speed : IMovable
    {
        private readonly float _value;

        public Speed(float value, float lifeTime = 0f)
        {
            _value = value;
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
        
        public void Accept(IBoosterVisitor visitor)
        {
            visitor.Visit(this);
        }

        public float GetSpeed()
        {
            return _value;
        }
    }
}