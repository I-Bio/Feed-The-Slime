using System;
using System.Collections;
using UnityEngine;

namespace Menu
{
    public class AutoSaveRequester : MonoBehaviour
    {
        private WaitForSeconds _wait;
        private Coroutine _routine;

        public event Action SaveRequested;
        
        public void Initialize(float delay)
        {
            _wait = new WaitForSeconds(delay);
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
            while (true)
            {
                yield return _wait;
                SaveRequested?.Invoke();
            }
        }
    }
}