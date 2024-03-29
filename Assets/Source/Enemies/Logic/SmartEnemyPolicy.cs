namespace Enemies
{
    public class SmartEnemyPolicy : IEnemyPolicy
    {
        public bool CanMove(bool isHidden)
        {
            return true;
        }
    }
}