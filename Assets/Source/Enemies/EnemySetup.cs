using System.Collections.Generic;
using Foods;
using Players;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(EnemyThinker))]
    [RequireComponent(typeof(EnemyAnimation))]
    public class EnemySetup : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private float _followDistance;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _thinkDelay;
        [SerializeField] private float _idleOffset = 0.1f;
        [SerializeField] private EnemyCollisionDetector _detector;
        [SerializeField] private FoodSetup _foodPart;
        

        [Space, Header("Enemy Type")] 
        [SerializeField] private EnemyTypes _type;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private Swarm _swarm;

        private EnemyThinker _thinker;
        private EnemyAnimation _animation;
        private Transform _transform;
        
        private FinalStateMachine _model;
        private EnemyPresenter _presenter;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _followDistance);
        }

        private void OnDestroy()
        {
            _presenter?.Disable();
        }
        
        public void Initialize(IHidden player, IPlayerVisitor visitor)
        {
            _transform = transform;
            _thinker = GetComponent<EnemyThinker>();
            _animation = GetComponent<EnemyAnimation>();

            _model = new FinalStateMachine();
            IFactory<Dictionary<EnemyStates, FinalStateMachineState>> factory =
                new EnemyStatesFactory(_type, _model, player, _transform, _animation, _foodPart.Stage, _followDistance,
                    _transform.position, _idleOffset, _agent, _sound, visitor, _particle, _swarm, _thinkDelay);
            _presenter = new EnemyPresenter(_model, _thinker);
            _model.AddStates(factory.Create());
            
            _thinker.Initialize(_thinkDelay);
            _animation.Initialize(_animator, () => _model.SetState(EnemyStates.Idle));
            _foodPart.Initialize(float.NaN, () => Destroy(gameObject));
            _detector.Initialize(_foodPart.Stage);

            _presenter.Enable();
        }
    }
}