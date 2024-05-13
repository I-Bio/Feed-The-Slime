namespace Boosters
{
    public class CombinedSpeed : Combined<IMovable>, IMovable
    {
        public float GetSpeed()
        {
            return Stat.GetSpeed();
        }
    }
}