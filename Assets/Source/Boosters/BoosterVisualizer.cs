using System;
using System.Collections;
using UnityEngine;

namespace Boosters
{
    public class BoosterVisualizer : MonoBehaviour, IBoosterVisitor
    {
        private float _delay;
        private ParticleSystem _speed;
        private ParticleSystem _score;
        private Coroutine _routine;

        public event Action<float> Updated;

        public void Initialize(float delay, ParticleSystem speed, ParticleSystem score)
        {
            _delay = delay;
            _speed = speed;
            _score = score;
            
            _routine = StartCoroutine(UpdateRoutine());
        }
        
        public void Visit(IStatBuffer boost)
        {
            Visit(boost as dynamic);
        }

        public void Visit(IMovable movable)
        {
            _speed.Play();
        }

        public void Visit(ICalculableScore calculableScore)
        {
            _score.Play();
        }
        
        private IEnumerator UpdateRoutine()
        {
            bool isWorking = true;
            var wait = new WaitForSeconds(_delay);
            
            while (isWorking == true)
            {
                yield return wait;
                Updated?.Invoke(_delay);
            }
        }
    }
}