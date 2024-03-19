using System;

namespace Boosters
{
    public class BoosterEjector : IBoosterVisitor
    {
        private readonly ICalculableScore _standardScore;
        private readonly IMovable _standardSpeed;

        public BoosterEjector(ICalculableScore standardScore, IMovable standardSpeed)
        {
            _standardScore = standardScore;
            _standardSpeed = standardSpeed;
        }
        
        public event Action<IMovable> SpeedEnded;
        public event Action<ICalculableScore> ScoreEnded;

        public void Visit(IStatBuffer boost)
        {
            Visit(boost as dynamic);
        }

        public void Visit(IMovable movable)
        {
            SpeedEnded?.Invoke(_standardSpeed);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            ScoreEnded?.Invoke(_standardScore);
        }
    }
}