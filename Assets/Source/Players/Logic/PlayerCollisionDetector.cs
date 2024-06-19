using System;
using System.Collections.Generic;
using Boosters;
using Enemies;
using UnityEngine;

namespace Players
{
    public class PlayerCollisionDetector : MonoBehaviour, IPlayerVisitor
    {
        private List<Contactable> _contactableObjects;
        private Collider _collider;
        private SatietyStage _stage;
        private bool _didInitialize;

        public event Action<float> ScoreGained;

        public event Action<IBooster> BoosterEntered;

        public event Action EnemyContacted;

        public event Action ToxinContacted;

        public event Action ContactStopped;

        private void FixedUpdate() => Detect();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IBooster booster) == false)
                return;

            BoosterEntered?.Invoke(booster);
        }

        public void Initialize(List<Contactable> contactableObjects, SatietyStage stage, Collider collider)
        {
            _contactableObjects = contactableObjects;
            _collider = collider;
            _stage = stage;
            _didInitialize = true;
        }

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

        public void Visit(IEatable edible, float score)
        {
            ScoreGained?.Invoke(score);
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = stage;
        }

        public void AddContactable(Contactable contactable)
        {
            if (_contactableObjects.Contains(contactable))
                return;

            _contactableObjects.Add(contactable);
        }

        private void Detect()
        {
            if (_didInitialize == false)
                return;

            foreach (Contactable contactable in _contactableObjects)
            {
                if (contactable == null)
                {
                    _contactableObjects.Remove(null);
                    continue;
                }

                contactable.TryContact(_collider.bounds);
            }
        }
    }
}