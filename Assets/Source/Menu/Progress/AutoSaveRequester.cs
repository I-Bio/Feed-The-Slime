using System;
using System.Collections;
using UnityEngine;

namespace Menu
{
    public class AutoSaveRequester : MonoBehaviour
    {
        private float _delay;
        private Coroutine _routine;

        public event Action SaveRequested;
        
        public void Initialize(float delay)
        {
            _delay = delay;
        }

        public void StartRequests()
        {
            _routine = StartCoroutine(RequestRoutine());
        }

        public void StopRequests()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        private IEnumerator RequestRoutine()
        {
            var wait = new WaitForSeconds(_delay);
            bool isWorking = true;

            while (isWorking == true)
            {
                yield return wait;
                SaveRequested?.Invoke();
            }
        }
    }
}