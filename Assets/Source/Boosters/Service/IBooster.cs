namespace Boosters
{
    public interface IBooster
    {
        public IStatBuffer GetBoost();
        public void Use();
    }
}