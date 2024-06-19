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
        [SerializeField] private FoodSetup _foodPart;
        [SerializeField] private EnemyCollisionDetector _detector;

        [Space] [Header("Enemy Type")]
        [SerializeField] private EnemyTypes _type;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private Swarm _swarm;

        private EnemyThinker _thinker;
        private EnemyAnimation _animation;
        private Transform _transform;

        private FiniteStateMachine _model;
        private EnemyPresenter _presenter;

        private void OnDestroy()
        {
            _presenter?.Disable();
        }

        public List<Contactable> Initialize(IHidden player, IPlayerVisitor visitor, out ISelectable selectable)
        {
            _transform = transform;
            _thinker = GetComponent<EnemyThinker>();
            _animation = GetComponent<EnemyAnimation>();

            _model = new FiniteStateMachine();
            IFactory<Dictionary<EnemyStates, FiniteStateMachineState>> factory =
                new EnemyStatesFactory(
                    _type,
                    _model,
                    player,
                    _transform,
                    _animation,
                    _foodPart.Stage,
                    _followDistance,
                    _transform.position,
                    _idleOffset,
                    _agent,
                    _sound,
                    visitor,
                    _particle,
                    _swarm);
            _presenter = new EnemyPresenter(_model, _thinker);
            _model.AddStates(factory.Create());

            _thinker.Initialize(_thinkDelay);
            _animation.Initialize(_animator, () => _model.SetState(EnemyStates.Idle));
            _detector.Initialize(_foodPart.Stage, visitor);
            Contactable contactable =
                _foodPart.Initialize(float.NaN, visitor, out selectable, () => Destroy(gameObject));

            _presenter.Enable();
            return new List<Contactable> { _detector, contactable };
        }
    }
}