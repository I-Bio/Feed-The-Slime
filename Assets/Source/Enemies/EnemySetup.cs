using Foods;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyMover))]
    [RequireComponent(typeof(EnemyAnimation))]
    public class EnemySetup : MonoBehaviour
    {
        [SerializeField] private float _followDistance;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idle;
        [SerializeField] private string _move;
        [SerializeField] private float _thinkDelay;
        
        [SerializeField] private EnemyCollisionDetector _detector;
        [SerializeField] private FoodSetup _foodPart;
        
        private EnemyMover _mover;
        private EnemyAnimation _animation;
        private Transform _transform;
        
        private Enemy _model;

        private EnemyPresenter _presenter;
        
        public void Initialize(IHidden player, IEnemyPolicy policy)
        {
            _transform = transform;
            _mover = GetComponent<EnemyMover>();
            _animation = GetComponent<EnemyAnimation>();

            _model = new Enemy(_transform, player, policy, _foodPart.Stage, _followDistance);
            _presenter = new EnemyPresenter(_model, _mover, _animation, _detector);
            
            _mover.Initialize(_thinkDelay);
            _animation.Initialize(_animator, _idle, _move);
            _foodPart.Initialize(float.NaN);
            
            _presenter.Enable();
        }

        private void OnDestroy()
        {
            _presenter.Disable();
        }
    }
}