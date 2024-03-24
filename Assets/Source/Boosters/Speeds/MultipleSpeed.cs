namespace Boosters
{
    public class MultipleSpeed : IMovable
    {
        private readonly IMovable _movable;
        private readonly float _scaler;

        public MultipleSpeed(IMovable movable, float scaler, float lifeTime)
        {
            _movable = movable;
            _scaler = scaler;
            LifeTime = lifeTime;
        }
        
        public float LifeTime { get; set; }
        
        public void Accept(IBoosterVisitor visitor)
        {
            visitor.Visit(this);
        }

        public float GetSpeed()
        {
            return _movable.GetSpeed() * _scaler;
        }
    }
}