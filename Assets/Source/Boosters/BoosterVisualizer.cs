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
        [SerializeField] private Transform _holder;

        private float _delay;
        private EffectReproducer _effectReproducer;
        private readonly List<KeyValuePair<SpawnableObject, IStatBuffer>> _boosters = new();

        public event Action<float> Updated;

        public void Initialize(float delay, EffectReproducer effectReproducer)
        {
            _delay = delay;
            _effectReproducer = effectReproducer;
            StartCoroutine(UpdateRoutine());
        }

        public void Visit(IMovable movable)
        {
            if (_boosters.Any(pair => pair.Value is IMovable == false))
            {
                _effectReproducer.PlayEffect(EffectType.SpeedBoost);
                _boosters.Add(
                    new KeyValuePair<SpawnableObject, IStatBuffer>(
                        PullAndSetParent<BoostIcon>(Vector3.zero, _holder)
                            .Initialize(movable.LifeTime, movable.Icon).Use(), movable));
                return;
            }

            Hide(movable);
        }

        public void Visit(ICalculableScore calculableScore)
        {
            if (_boosters.Any(pair => pair.Value is ICalculableScore != false))
            {
                _effectReproducer.PlayEffect(EffectType.ScoreBoost);
                _boosters.Add(
                    new KeyValuePair<SpawnableObject, IStatBuffer>(
                        PullAndSetParent<BoostIcon>(Vector3.zero, _holder)
                            .Initialize(calculableScore.LifeTime, calculableScore.Icon).Use(), calculableScore));
                return;
            }

            Hide(calculableScore);
        }

        private void Hide(IStatBuffer boost)
        {
            KeyValuePair<SpawnableObject, IStatBuffer> buff = _boosters.First(pair => pair.Value == boost);
            Push(buff.Key);
            _boosters.Remove(buff);
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