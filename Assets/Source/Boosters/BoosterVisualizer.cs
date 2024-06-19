using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spawners;
using UnityEngine;

namespace Boosters
{
    public class BoosterVisualizer : ObjectPool, IBoosterVisitor
    {
        private readonly List<KeyValuePair<SpawnableObject, IStat>> Boosters = new ();

        private float _delay;
        private WaitForSeconds _wait;
        private Dictionary<Type, Action> _effects;
        private IIconFactory _factory;

        public event Action<float> Updated;

        public void Initialize(float delay, BoostIcon icon, IIconFactory factory, Dictionary<Type, Action> effects)
        {
            Initialize(icon);
            _delay = delay;
            _wait = new WaitForSeconds(_delay);
            _effects = effects;
            _factory = factory;
            StartCoroutine(UpdateRoutine());
        }

        public void Visit(IMovable movable)
        {
            if (Boosters.Count == 0 || Boosters.Where(pair => pair.Value is IMovable).ToList().Count == 0)
            {
                if (_effects.TryGetValue(typeof(IMovable), out Action onGotSpeed) == false)
                    throw new NullReferenceException(nameof(IMovable));

                onGotSpeed.Invoke();
                SpawnIcon(movable as IStat);
                return;
            }

            Hide<IMovable>();
        }

        public void Visit(ICalculableScore calculableScore)
        {
            if (Boosters.Count == 0 || Boosters.Where(pair => pair.Value is ICalculableScore).ToList().Count == 0)
            {
                if (_effects.TryGetValue(typeof(ICalculableScore), out Action onGotScore) == false)
                    throw new NullReferenceException(nameof(ICalculableScore));

                onGotScore.Invoke();
                SpawnIcon(calculableScore as IStat);
                return;
            }

            Hide<ICalculableScore>();
        }

        private void SpawnIcon(IStat stat)
        {
            Boosters.Add(_factory.Create(stat));
        }

        private void Hide<T>()
        {
            KeyValuePair<SpawnableObject, IStat> boost = Boosters.First(pair => pair.Value is T);
            Push(boost.Key);
            Boosters.Remove(boost);
        }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                yield return _wait;
                Updated?.Invoke(_delay);
            }
        }
    }
}