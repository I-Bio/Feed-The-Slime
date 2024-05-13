using System;
using Players;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(AudioSource))]
    public class EnemyToxin : EnemyBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        
        private AudioSource _source;
        private IPlayerVisitor _player;
        private bool _canReproduce;
        
        private void OnDestroy()
        {
            _player?.Visit(null as EnemyEmpty);
        }
        
        public void Initialize(float delay, IPlayerVisitor player)
        {
            _player = player;
            _source = GetComponent<AudioSource>();
            Initialize(delay);
        }

        public override void Accept(IEnemyVisitor visitor, float thinkDelay)
        {
            visitor.Visit(this, thinkDelay);
        }

        public override void InteractInClose(Vector3 position)
        {
            if (_canReproduce == false)
            {
                _particle.Play();
                _source.Play();
                _canReproduce = true;
            }

            _player.Visit(this);
        }

        public override void AvoidInteraction(Vector3 position, Action onAvoided) => CancelInteraction();

        public override void CancelInteraction()
        {
            if (_canReproduce == false)
                return;
            
            _canReproduce = false;
            _player.Visit(null as EnemyEmpty);
            _source.Stop();
            _particle.Stop();
        }
    }
}