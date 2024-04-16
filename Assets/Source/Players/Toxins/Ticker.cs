using System;
using System.Collections;
using UnityEngine;

namespace Players
{
    public class Ticker : MonoBehaviour
    {
        private float _delay;

        private Coroutine _routine;
        
        public event Action Ticked;

        public void Initialize(float delay = 1f)
        {
            _delay = delay;
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
            bool isWorking = true;
            var wait = new WaitForSeconds(_delay);
            
            while (isWorking == true)
            {
                yield return wait;
                Ticked?.Invoke();
            }
        }
    }
}