using System;
using UnityEngine;

namespace Players
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        public event Action<float> ScoreGained;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IEatable eatable) == false)
                return;
            
            if (eatable.TryEat(out float score) == false)
                return;
            
            ScoreGained?.Invoke(score);
        }
    }
}