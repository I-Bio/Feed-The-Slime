using Players;

namespace Enemies
{
    public class EnemyDependencyVisitor : IEnemyVisitor
    {
        private readonly IPlayerVisitor _visitor;

        public EnemyDependencyVisitor(IPlayerVisitor visitor)
        {
            _visitor = visitor;
        }
        
        public void Visit(EnemyMover mover, float thinkDelay)
        {
            mover.Initialize(thinkDelay);
        }

        public void Visit(EnemyToxin toxin, float thinkDelay)
        {
            toxin.Initialize(thinkDelay, _visitor);
        }

        public void Visit(EnemySwarm swarm, float thinkDelay)
        {
            swarm.Initialize(thinkDelay, _visitor);
        }
    }
}