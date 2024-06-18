using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyThinker : MonoBehaviour
    {
        private Coroutine _routine;
        private WaitForSeconds _wait;

        public event Action GoingThink;

        public void Initialize(float delay)
        {
            _wait = new WaitForSeconds(delay);
        }

        public void StartTick()
        {
            _routine = StartCoroutine(TickRoutine());
        }

        public void StopTick()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        private IEnumerator TickRoutine()
        {
            while (true)
            {
                yield return _wait;
                GoingThink?.Invoke();
            }
        }
    }
}