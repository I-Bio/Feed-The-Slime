namespace Enemies
{
    public interface IEnemyVisitor
    {
        public void Visit(EnemyMover mover, float thinkDelay);
        public void Visit(EnemyToxin toxin, float thinkDelay);
        public void Visit(EnemySwarm swarm, float thinkDelay);
    }
}