namespace Boosters
{
    public class AdditionalSpeed : IMovable
    {
        private readonly IMovable _movable;
        private readonly float _additionValue;

        public AdditionalSpeed(IMovable movable, float additionValue, float lifeTime)
        {
            _movable = movable;
            _additionValue = additionValue;
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
        
        public float GetSpeed()
        {
            return _movable.GetSpeed() + _additionValue;
        }
    }
}