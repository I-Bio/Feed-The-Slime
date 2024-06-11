using System;
using System.Collections;
using UnityEngine;

namespace Players
{
    public class Ticker : MonoBehaviour
    {
        private WaitForSeconds _wait;
        private Coroutine _routine;
        
        public event Action Ticked;

        public void Initialize(float delay = 1f)
        {
            _wait = new WaitForSeconds(delay);
        }
        
        public void StartTick()
        {
            _routine = StartCoroutine(TickRoutine());
        }

        public void Stop()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        private IEnumerator TickRoutine()
        {
            while (true)
            {
                yield return _wait;
                Ticked?.Invoke();
            }
        }
    }
}