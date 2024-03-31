using System;
using System.Collections;
using Players;
using UnityEngine;

namespace Boosters
{
    public class BoosterVisualizer : MonoBehaviour, IBoosterVisitor
    {
        private float _delay;
        private EffectReproducer _effectReproducer;
        private Coroutine _routine;

        public event Action<float> Updated;

        public void Initialize(float delay, EffectReproducer effectReproducer)
        {
            _delay = delay;
            _effectReproducer = effectReproducer;
            _routine = StartCoroutine(UpdateRoutine());
        }

        public void Visit(IMovable movable)
        {
            _effectReproducer.PlayEffect(EffectType.SpeedBoost);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            _effectReproducer.PlayEffect(EffectType.ScoreBoost);
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