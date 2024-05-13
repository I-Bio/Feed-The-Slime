using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        private float _delay;

        public event Action GoingThink;

        public void Initialize(float delay)
        {
            _delay = delay;
            StartCoroutine(ThinkRoutine());
        }

        public abstract void Accept(IEnemyVisitor visitor, float thinkDelay);

        public abstract void InteractInClose(Vector3 position);
        
        public abstract void AvoidInteraction(Vector3 position, Action onAvoided);
        
        public abstract void CancelInteraction();

        private IEnumerator ThinkRoutine()
        {
            bool isWorking = true;
            var wait = new WaitForSeconds(_delay);

            while (isWorking)
            {
                GoingThink?.Invoke();
                yield return wait;
            }
        }
    }
}