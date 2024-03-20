using Spawners;
using UnityEngine;

namespace Foods
{
    public class EdiblePart : MonoBehaviour, IEatable
    {
        private float _scorePerEat;
        private bool _isAllowed;
        
        public void Initialize(float scorePerEat)
        {
            _scorePerEat = scorePerEat;
        }

        public void Allow()
        {
            _isAllowed = true;
        }

        public bool TryEat(out float score)
        {
            score = 0f;
            
            if (_isAllowed == false)
                return false;

            score = _scorePerEat;
            Destroy(gameObject);
            return true;
        }
        
    }
}