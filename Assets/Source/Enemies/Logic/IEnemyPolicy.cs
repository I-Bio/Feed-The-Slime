namespace Enemies
{
    public interface IEnemyPolicy
    {
        public bool CanMove(bool isHidden);
    }
}