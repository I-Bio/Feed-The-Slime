namespace Enemies
{
    public class NormalEnemyPolicy : IEnemyPolicy
    {
        public bool CanMove(bool isHidden)
        {
            return isHidden;
        }
    }
}