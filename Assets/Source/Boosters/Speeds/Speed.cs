namespace Boosters
{
    public class Speed : IMovable
    {
        private readonly float _value;

        public Speed(float value, float lifeTime)
        {
            _value = value;
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
            
        public float GetSpeed()
        {
            return _value;
        }
    }
}