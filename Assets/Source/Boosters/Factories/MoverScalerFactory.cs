namespace Boosters
{
    public class MoverScalerFactory : IMovableFactory
    {
        private readonly IMovable Start;
        private readonly float ScaleFactor;

        private IMovable _current;
        
        public MoverScalerFactory(IMovable start, float scaleFactor)
        {
            Start = start;
            ScaleFactor = scaleFactor;
            _current = Start;
        }
        
        public IMovable Create(SatietyStage stage)
        {
            float additional = Start.GetSpeed() + ScaleFactor * (float)stage - _current.GetSpeed();
            _current = new AdditionalSpeed(_current, additional);
            return _current;
        }
    }
}