namespace Boosters
{
    public interface IBooster
    {
        public IStat GetBoost();

        public void Use();
    }
}