using Foods;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyBehaviour))]
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
        
        private EnemyBehaviour _behaviour;
        private EnemyAnimation _animation;
        private Transform _transform;
        
        private Enemy _model;
        private EnemyPresenter _presenter;
        
        public void Initialize(IHidden player, EnemyDependencyVisitor visitor, IEnemyPolicy policy)
        {
            _transform = transform;
            _behaviour = GetComponent<EnemyBehaviour>();
            _animation = GetComponent<EnemyAnimation>();

            _model = new Enemy(_transform, player, policy, _foodPart.Stage, _followDistance);
            _presenter = new EnemyPresenter(_model, _behaviour, _animation, _detector);

            _behaviour.Accept(visitor, _thinkDelay);
            _animation.Initialize(_animator, _idle, _move);
            _foodPart.Initialize(float.NaN, () => Destroy(gameObject));
            _detector.Initialize(_foodPart.Stage);
            
            _presenter.Enable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _followDistance);
        }

        private void OnDestroy()
        {
            _presenter.Disable();
        }
    }
}