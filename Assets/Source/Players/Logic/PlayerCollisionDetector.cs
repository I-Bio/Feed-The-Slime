using System;
using Boosters;
using Enemies;
using UnityEngine;

namespace Players
{
    public class PlayerCollisionDetector : MonoBehaviour, IPlayerVisitor
    {
        private SatietyStage _stage;
        
        public event Action<float> ScoreGained;
        public event Action<IBooster> BoosterEntered;
        public event Action EnemyContacted;
        public event Action ToxinContacted;
        public event Action ContactStopped;

        public void Visit(EnemyKiller killer, SatietyStage stage)
        {
            if (stage <= _stage)
                return;
            
            EnemyContacted?.Invoke();
        }

        public void Visit(EnemyAttackState toxin)
        {
            ToxinContacted?.Invoke();
        }

        public void Visit(EnemyEmpty empty)
        {
            ContactStopped?.Invoke();
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = stage;
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