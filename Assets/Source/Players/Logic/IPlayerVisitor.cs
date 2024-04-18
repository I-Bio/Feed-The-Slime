using System;
using Enemies;

namespace Players
{
    public interface IPlayerVisitor
    {
        public event Action ToxinContacted;
        public event Action ContactStopped;
        
        public void Visit(EnemyMover normal, SatietyStage stage);
        public void Visit(EnemyToxin toxin);
        public void Visit(EnemyEmpty empty);
    }
}