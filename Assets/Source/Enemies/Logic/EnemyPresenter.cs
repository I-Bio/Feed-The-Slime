using UnityEngine;

namespace Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly Enemy _model;
        private readonly EnemyMover _mover;
        private readonly EnemyAnimation _animation;
        private readonly EnemyCollisionDetector _detector;
        
        public EnemyPresenter(Enemy model, EnemyMover mover, EnemyAnimation animation, EnemyCollisionDetector detector)
        {
            _model = model;
            _mover = mover;
            _animation = animation;
            _detector = detector;
        }
        
        public void Enable()
        {
            _model.Moved += OnMoved;
            _model.Idled += OnIdled;
            _model.RunningAway += OnRunningAway;

            _mover.GoingMove += OnGoingMove;
        }

        public void Disable()
        {
            _model.Moved -= OnMoved;
            _model.Idled -= OnIdled;
            _model.RunningAway -= OnRunningAway;

            _mover.GoingMove -= OnGoingMove;
        }

        private void OnMoved(Vector3 position)
        {
            _animation.PlayMove();
            _mover.Move(position);
        }

        private void OnIdled()
        {
            _animation.PlayIdle();
        }

        private void OnRunningAway(Vector3 position)
        {
            _detector.DisallowContact();
            OnMoved(position);
        }

        private void OnGoingMove()
        {
            _model.CompareDistance();
        }
    }
}