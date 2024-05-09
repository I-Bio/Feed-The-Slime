using UnityEngine;

namespace Boosters
{
    public class BoosterPresenter : IPresenter, IBoosterVisitor
    {
        private readonly ISettable<ICalculableScore> _scoreHolder;
        private readonly ISettable<IMovable> _speedHolder;
        private readonly BoosterService _service;
        private readonly BoosterVisualizer _visualizer;

        public BoosterPresenter(ISettable<ICalculableScore> scoreHolder, ISettable<IMovable> speedHolder, BoosterService service,
            BoosterVisualizer visualizer)
        {
            _scoreHolder = scoreHolder;
            _speedHolder = speedHolder;
            _service = service;
            _visualizer = visualizer;
        }

        public void Enable()
        {
            _service.Injected += OnBoosterStateChanged;
            _service.Ejected += OnBoosterStateChanged;

            _visualizer.Updated += OnUpdated;
        }

        public void Disable()
        {
            _service.Injected -= OnBoosterStateChanged;
            _service.Ejected -= OnBoosterStateChanged;

            _visualizer.Updated -= OnUpdated;
        }
        
        private void OnBoosterStateChanged(IStat boost)
        {
            Debug.Log(boost.ToString());
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