namespace Boosters
{
    public class BoosterPresenter : IPresenter, IBoosterVisitor
    {
        private readonly ISettable _scoreHolder;
        private readonly ISettable _speedHolder;
        private readonly BoosterService _service;
        private readonly BoosterVisualizer _visualizer;
        private readonly BoosterEjector _ejector;

        public BoosterPresenter(ISettable scoreHolder, ISettable speedHolder, BoosterService service, BoosterVisualizer visualizer, BoosterEjector ejector)
        {
            _scoreHolder = scoreHolder;
            _speedHolder = speedHolder;
            _service = service;
            _visualizer = visualizer;
            _ejector = ejector;
        }

        public void Enable()
        {
            _service.Injected += OnBoosterAccept;
            _service.Ejected += OnEjected;
            _ejector.Completed += OnBoosterAccept;
            
            _visualizer.Updated += OnUpdated;
        }

        public void Disable()
        {
            _service.Injected -= OnBoosterAccept;
            _service.Ejected -= OnEjected;
            _ejector.Completed -= OnBoosterAccept;

            _visualizer.Updated -= OnUpdated;
        }

        private void OnEjected(IStatBuffer boost)
        {
            boost.Accept(_ejector);
        }

        private void OnBoosterAccept(IStatBuffer boost)
        {
            boost.Accept(this);
        }

        private void OnUpdated(float delay)
        {
            _service.Update(delay);
        }

        public void Visit(IMovable movable)
        {
            _speedHolder.SetBoost(movable);
            _visualizer.Visit(movable);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            _scoreHolder.SetBoost(calculableScore);
            _visualizer.Visit(calculableScore);
        }
    }
}