using UnityEngine;

namespace Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly Enemy _model;
        private readonly EnemyMover _mover;
        private readonly EnemyAnimation _animation;
        
        public EnemyPresenter(Enemy model, EnemyMover mover, EnemyAnimation animation)
        {
            _model = model;
            _mover = mover;
            _animation = animation;
        }
        
        public void Enable()
        {
            _model.Moved += OnMoved;
            _model.Idled += OnIdled;

            _mover.GoingMove += OnGoingMove;
        }

        public void Disable()
        {
            _model.Moved -= OnMoved;
            _model.Idled -= OnIdled;

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

        private void OnGoingMove()
        {
            _model.CompareDistance();
        }
    }
}