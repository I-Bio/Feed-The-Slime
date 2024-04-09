using System;

namespace Boosters
{
    public class BoosterEjector : IBoosterVisitor
    {
        private readonly IMovable _standardSpeed;
        private readonly ICalculableScore _standardScore;

        public BoosterEjector(IMovable standardSpeed, ICalculableScore standardScore)
        {
            _standardSpeed = standardSpeed;
            _standardScore = standardScore;
        }
        
        public event Action<IStatBuffer> Completed;

        public void Visit(IMovable movable)
        {
            Completed?.Invoke(_standardSpeed);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            Completed?.Invoke(_standardScore);
        }
    }
}