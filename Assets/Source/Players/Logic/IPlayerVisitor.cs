using System;
using Enemies;

namespace Players
{
    public interface IPlayerVisitor
    {
        public event Action ToxinContacted;

        public event Action ContactStopped;

        public void Visit(EnemyKiller killer, SatietyStage stage);

        public void Visit(EnemyAttackState toxin);

        public void Visit(EnemyEmpty empty);

        public void Visit(IEatable eatable, float score);
    }
}