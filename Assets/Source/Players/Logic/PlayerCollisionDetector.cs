using System;
using Boosters;
using UnityEngine;

namespace Players
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        public event Action<float> ScoreGained;
        public event Action<IBooster> BoosterEntered;
        public event Action EnemyContacted;

        public void ContactEnemy()
        {
            EnemyContacted?.Invoke();
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IEatable eatable) == false)
                return;
            
            if (eatable.TryEat(out float score) == false)
                return;
            
            ScoreGained?.Invoke(score);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IBooster booster) == false)
                return;
            
            BoosterEntered?.Invoke(booster);
        }
    }
}