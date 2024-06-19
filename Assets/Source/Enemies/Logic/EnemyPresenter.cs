namespace Enemies
{
    public class EnemyPresenter
    {
        private readonly FiniteStateMachine Model;
        private readonly EnemyThinker Thinker;

        public EnemyPresenter(FiniteStateMachine model, EnemyThinker thinker)
        {
            Model = model;
            Thinker = thinker;
        }

        public void Enable()
        {
            Thinker.GoingThink += OnGoingThink;

            Thinker.StartTick();
        }

        public void Disable()
        {
            Thinker.GoingThink -= OnGoingThink;

            Thinker.StopTick();
            Model.Exit();
        }

        private void OnGoingThink()
        {
            Model.Update();
        }
    }
}