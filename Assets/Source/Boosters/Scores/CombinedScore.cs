namespace Boosters
{
    public class CombinedScore : Combined<ICalculableScore>, ICalculableScore
    {
        public float CalculateScore(float score)
        {
            return Next.CalculateScore(score);
        }
    }
}