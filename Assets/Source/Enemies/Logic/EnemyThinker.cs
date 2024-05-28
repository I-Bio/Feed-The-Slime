using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyThinker : MonoBehaviour
    {
        private float _delay;
        private Coroutine _routine;

        public event Action GoingThink;

        public void Initialize(float delay)
        {
            _delay = delay;
        }

        public void StartThink()
        {
            _routine = StartCoroutine(ThinkRoutine());
        }

        public void StopThink()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        private IEnumerator ThinkRoutine()
        {
            bool isWorking = true;
            var wait = new WaitForSeconds(_delay);

            while (isWorking)
            {
                yield return wait;
                GoingThink?.Invoke();
            }
        }
    }
}