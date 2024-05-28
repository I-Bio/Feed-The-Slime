namespace Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly FinalStateMachine Model;
        private readonly EnemyThinker Thinker;

        public EnemyPresenter(FinalStateMachine model, EnemyThinker thinker)
        {
            Model = model;
            Thinker = thinker;
        }

        public void Enable()
        {
            Thinker.GoingThink += OnGoingThink;
            
            Thinker.StartThink();
        }

        public void Disable()
        {
            Thinker.GoingThink -= OnGoingThink;
            
            Thinker.StopThink();
            Model.Exit();
        }
        
        private void OnGoingThink()
        {
            Model.Update();
        }
    }
}