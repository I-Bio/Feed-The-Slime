namespace Boosters
{
    public interface IInsertable
    {
        public bool TryInsert(IBooster booster);
    }
}