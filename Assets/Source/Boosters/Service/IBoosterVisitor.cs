namespace Boosters
{
    public interface IBoosterVisitor
    {
        public void Visit(IStatBuffer boost);
        public void Visit(IMovable movable);
        public void Visit(ICalculableScore calculableScore);
    }
}