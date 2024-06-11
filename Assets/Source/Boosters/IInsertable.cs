namespace Boosters
{
    public interface IInsertable<in T>
    {
        public void Insert(T stat);
    }
}