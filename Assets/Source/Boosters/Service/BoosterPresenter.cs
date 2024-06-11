namespace Boosters
{
    public class BoosterPresenter : IBoosterVisitor
    {
        private readonly ISettable<ICalculableScore> ScoreHolder;
        private readonly ISettable<IMovable> SpeedHolder;
        private readonly BoosterService Service;
        private readonly BoosterVisualizer Visualizer;

        public BoosterPresenter(ISettable<ICalculableScore> scoreHolder, ISettable<IMovable> speedHolder,
            BoosterService service, BoosterVisualizer visualizer)
        {
            ScoreHolder = scoreHolder;
            SpeedHolder = speedHolder;
            Service = service;
            Visualizer = visualizer;
        }

        public void Enable()
        {
            Service.Injected += OnBoosterStateChanged;
            Service.Ejected += OnBoosterStateChanged;
            Visualizer.Updated += OnUpdated;
        }

        public void Disable()
        {
            Service.Injected -= OnBoosterStateChanged;
            Service.Ejected -= OnBoosterStateChanged;
            Visualizer.Updated -= OnUpdated;
        }

        public void Visit(IMovable movable)
        {
            SpeedHolder.SetBoost(movable);
            Visualizer.Visit(movable);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            ScoreHolder.SetBoost(calculableScore);
            Visualizer.Visit(calculableScore);
        }
        
        private void OnBoosterStateChanged(IStat boost)
        {
            boost.Accept(this);
        }

        private void OnUpdated(float delay)
        {
            Service.Update(delay);
        }
    }
}