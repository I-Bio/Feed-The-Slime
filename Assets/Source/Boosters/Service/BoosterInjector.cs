using System;

namespace Boosters
{
    public class BoosterInjector : IBoosterVisitor
    {
        public event Action<IMovable> SpeedBoosterGained;
        public event Action<ICalculableScore> ScoreBoosterGained;

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