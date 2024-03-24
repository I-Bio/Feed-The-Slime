namespace Boosters
{
    public class BoosterPresenter : IPresenter
    {
        private readonly ISettable _scoreHolder;
        private readonly ISettable _speedHolder;
        private readonly BoosterInjector _injector;
        private readonly BoosterEjector _ejector;
        private readonly BoosterService _service;
        private readonly BoosterVisualizer _visualizer;

        public BoosterPresenter(ISettable scoreHolder, ISettable speedHolder, BoosterInjector injector,
            BoosterEjector ejector, BoosterService service, BoosterVisualizer visualizer)
        {
            _scoreHolder = scoreHolder;
            _speedHolder = speedHolder;
            _injector = injector;
            _ejector = ejector;
            _service = service;
            _visualizer = visualizer;
        }

        public void Enable()
        {
            _service.Injected += OnGoingInject;
            _service.Ejected += OnGoingEject;
            
            _injector.ScoreBoosterGained += OnScoreGained;
            _injector.SpeedBoosterGained += OnSpeedGained;

            _ejector.ScoreEnded += OnScoreReset;
            _ejector.SpeedEnded += OnSpeedReset;
            
            _visualizer.Updated += OnUpdated;
        }

        public void Disable()
        {
            _service.Injected -= OnGoingInject;
            _service.Ejected -= OnGoingEject;
            
            _injector.ScoreBoosterGained -= OnScoreGained;
            _injector.SpeedBoosterGained -= OnSpeedGained;

            _ejector.ScoreEnded -= OnScoreReset;
            _ejector.SpeedEnded -= OnSpeedReset;

            _visualizer.Updated -= OnUpdated;
        }

        private void OnGoingInject(IStatBuffer boost)
        {
            _injector.Visit(boost);
        }

        private void OnGoingEject(IStatBuffer boost)
        {
            _ejector.Visit(boost);
        }

        private void OnScoreGained(ICalculableScore calculableScore)
        {
            _scoreHolder.SetBoost(calculableScore);
            _visualizer.Visit(calculableScore);
        }
        
        private void OnScoreReset(ICalculableScore calculableScore)
        {
            _scoreHolder.SetBoost(calculableScore);
        }

        private void OnSpeedGained(IMovable movable)
        {
            _speedHolder.SetBoost(movable);
            _visualizer.Visit(movable);
        }
        
        private void OnSpeedReset(IMovable movable)
        {
            _speedHolder.SetBoost(movable);
        }

        private void OnUpdated(float delay)
        {
            _service.Update(delay);
        }
    }
}