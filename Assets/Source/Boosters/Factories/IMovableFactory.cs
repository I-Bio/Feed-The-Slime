namespace Boosters
{
    public interface IMovableFactory
    {
        public IMovable Create(SatietyStage stage);
    }
}