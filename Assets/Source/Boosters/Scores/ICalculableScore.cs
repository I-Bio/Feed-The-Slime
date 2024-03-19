namespace Boosters
{
    public interface ICalculableScore : IStatBuffer
    {
        public float CalculateScore(float score);
    }
}