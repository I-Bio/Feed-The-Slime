using UnityEngine;

namespace Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly Enemy _model;
        private readonly EnemyBehaviour _behaviour;
        private readonly EnemyAnimation _animation;
        private readonly EnemyCollisionDetector _detector;

        public EnemyPresenter(Enemy model, EnemyBehaviour behaviour, EnemyAnimation animation,
            EnemyCollisionDetector detector)
        {
            _model = model;
            _behaviour = behaviour;
            _animation = animation;
            _detector = detector;
        }

        public void Enable()
        {
            _model.GoingInteract += OnGoingInteract;
            _model.Canceled += OnCanceled;
            _model.Avoided += OnAvoided;

            _behaviour.GoingThink += OnGoingThink;
        }

        public void Disable()
        {
            _model.GoingInteract -= OnGoingInteract;
            _model.Canceled -= OnCanceled;
            _model.Avoided -= OnAvoided;

            _behaviour.GoingThink -= OnGoingThink;
        }

        private void OnGoingInteract(Vector3 position)
        {
            _animation.PlayMove();
            _detector.AllowContact();
            _behaviour.InteractInClose(position);
        }

        private void OnCanceled()
        {
            _animation.PlayIdle();
            _behaviour.CancelInteraction();
        }

        private void OnAvoided(Vector3 position)
        {
            _detector.DisallowContact();
            _behaviour.AvoidInteraction(position, _animation.PlayMove);
        }

        private void OnGoingThink()
        {
            _model.CompareDistance();
        }
    }
}