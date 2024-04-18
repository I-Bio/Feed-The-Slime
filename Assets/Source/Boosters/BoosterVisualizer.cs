using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Players;
using Spawners;
using UnityEngine;

namespace Boosters
{
    public class BoosterVisualizer : ObjectPool, IBoosterVisitor
    {
        private readonly List<KeyValuePair<SpawnableObject, IStatBuffer>> _boosters = new();
        
        private float _delay;
        private Transform _holder;
        private  Dictionary<Type, Action> _effects;

        public event Action<float> Updated;

        public void Initialize(float delay, Dictionary<Type, Action> effects, Transform holder, BoostIcon icon)
        {
            Initialize(icon);
            _holder = holder;
            _delay = delay;
            _effects = effects;
            StartCoroutine(UpdateRoutine());
        }

        public void Visit(IMovable movable)
        {
            if (_boosters.Count == 0 || _boosters.Where(pair => pair.Value is IMovable == true).ToList().Count == 0)
            {
                if (_effects.TryGetValue(typeof(IMovable), out Action onGotSpeed) == false)
                    throw new NullReferenceException(nameof(IMovable));
                
                onGotSpeed.Invoke();
                SpawnIcon(movable);
                return;
            }

            Hide<IMovable>();
        }

        public void Visit(ICalculableScore calculableScore)
        {
            if (_boosters.Count == 0 || _boosters.Where(pair => pair.Value is ICalculableScore == true).ToList().Count == 0)
            {
                if (_effects.TryGetValue(typeof(ICalculableScore), out Action onGotScore) == false)
                    throw new NullReferenceException(nameof(ICalculableScore));
                
                onGotScore.Invoke();
                SpawnIcon(calculableScore);
                return;
            }

            Hide<ICalculableScore>();
        }

        private void SpawnIcon(IStatBuffer boost)
        {
            _boosters.Add(
                new KeyValuePair<SpawnableObject, IStatBuffer>(
                    Pull<BoostIcon>(_holder)
                        .Initialize(boost.LifeTime, boost.Icon).Use(), boost));
        }

        private void Hide<T>()
        {
            KeyValuePair<SpawnableObject, IStatBuffer> boost = _boosters.First(pair => pair.Value is T);
            Push(boost.Key);
            _boosters.Remove(boost);
        }

        private IEnumerator UpdateRoutine()
        {
            bool isWorking = true;
            var wait = new WaitForSeconds(_delay);

            while (isWorking == true)
            {
                yield return wait;
                Updated?.Invoke(_delay);
            }
        }
    }
}