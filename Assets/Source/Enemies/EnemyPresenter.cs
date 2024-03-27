using UnityEngine;

namespace Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly Enemy _model;
        private readonly EnemyMover _mover;
        
        public EnemyPresenter(Enemy model, EnemyMover mover)
        {
            _model = model;
            _mover = mover;
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
            _mover.Move(position);
        }

        private void OnIdled()
        {
            
        }

        private void OnGoingMove()
        {
            _model.CompareDistance();
        }
    }
}