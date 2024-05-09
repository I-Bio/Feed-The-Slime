namespace Boosters
{
    public class MoverScalerFactory : IFactory<IMovable>
    {
        private readonly IMovable Start;
        private readonly float ScaleFactor;

        private IMovable _current;
        private SatietyStage _stage;
        
        public MoverScalerFactory(IMovable start, float scaleFactor)
        {
            Start = start;
            ScaleFactor = scaleFactor;
            _current = Start;
            _stage = SatietyStage.Exhaustion;
        }
        
        public IMovable Create()
        {
            _stage++;
            float additional = Start.GetSpeed() * ScaleFactor * (float)_stage - _current.GetSpeed();
            
            _current = new AdditionalSpeed(_current, additional);
            return _current;
        }
    }
}