using System;
using System.Collections;
using UnityEngine;

namespace Players
{
    public class Ticker : MonoBehaviour
    {
        [SerializeField] private float _delay = 1f;

        private Coroutine _routine;
        
        public event Action Ticked;

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