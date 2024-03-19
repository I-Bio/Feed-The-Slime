using System;

namespace Boosters
{
    public class BoosterInjector : IBoosterVisitor
    {
        public event Action<IMovable> SpeedBoosterGained;
        public event Action<ICalculableScore> ScoreBoosterGained;

        public void Visit(IStatBuffer boost)
        {
            Visit(boost as dynamic);
        }

        public void Visit(IMovable movable)
        {
            SpeedBoosterGained?.Invoke(movable);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            ScoreBoosterGained?.Invoke(calculableScore);
        }
    }
}