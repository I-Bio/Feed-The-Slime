using System;
using Players;
using UnityEngine;

namespace Enemies
{
    public class EnemySwarm : EnemyBehaviour
    {
        [SerializeField] private Swarm _swarm;
        
        private IPlayerVisitor _player;
        private bool _didHide;

        private void OnDestroy()
        {
            _player?.Visit(null as EnemyEmpty);
        }

        public void Initialize(float delay, IPlayerVisitor player)
        {
            _player = player;
            _swarm.Initialize(delay);
            _swarm.Hide();
            _didHide = true;
            base.Initialize(delay);
        }

        public override void Accept(IEnemyVisitor visitor, float thinkDelay)
        {
            visitor.Visit(this, thinkDelay);
        }

        public override void InteractInClose(Vector3 position)
        {
            if (_didHide == true)
            {
                _didHide = false;
                _swarm.Show();
            }
            
            _swarm.Move(position);
            _player.Visit(null as EnemyToxin);
        }

        public override void AvoidInteraction(Vector3 position, Action onAvoided) => CancelInteraction();

        public override void CancelInteraction()
        {
            if (_didHide == true)
                return;

            _didHide = true;
            _player.Visit(null as EnemyEmpty);
            _swarm.Hide();
        }
    }
}