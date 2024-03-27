using Foods;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyMover))]
    [RequireComponent(typeof(FoodSetup))]
    public class EnemySetup : MonoBehaviour
    {
        [SerializeField] private float _followDistance;

        private EnemyMover _mover;
        private Transform _transform;
        
        private Enemy _model;

        private EnemyPresenter _presenter;
        
        public void Initialize(IHidden player, IEnemyPolicy policy)
        {
            _transform = transform;
            _mover = GetComponent<EnemyMover>();

            _model = new Enemy(_transform, player, policy, _transform.position, _followDistance);
            _presenter = new EnemyPresenter(_model, _mover);
            
            _mover.Initialize();
            
            _presenter.Enable();
        }

        private void OnDestroy()
        {
            _presenter.Disable();
        }
    }
}