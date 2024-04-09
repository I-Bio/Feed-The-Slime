using System;
using System.Collections.Generic;
using System.Linq;

namespace Boosters
{
    public class BoosterService : IBoosterVisitor, IInsertable
    {
        private readonly List<SerializedPair<IStatBuffer, float>> _currentBoosters = new();
        private readonly List<SerializedPair<IStatBuffer, float>> _toDeleteBoosters = new();

        private bool _isAllowBoost;

        public event Action<IStatBuffer> Ejected; 
        public event Action<IStatBuffer> Injected; 

        public void Update(float delay)
        {
            foreach (SerializedPair<IStatBuffer, float> pair in _currentBoosters)
            {
                var booster = pair;
                booster.Value -= delay;
                
                if (booster.Value <= booster.Key.LifeTime)
                    _toDeleteBoosters.Add(booster);
            }

            foreach (SerializedPair<IStatBuffer, float> pair in _toDeleteBoosters)
            {
                Ejected?.Invoke(pair.Key);
                _currentBoosters.Remove(pair);
            }
            
            _toDeleteBoosters.Clear();
        }

        public bool TryInsert(IBooster booster)
        {
            IStatBuffer boost = booster.GetBoost();
            
            boost.Accept(this);

            if (_isAllowBoost == false)
                return false;
            
            _isAllowBoost = false;
            
            booster.Use();
            _currentBoosters.Add(new SerializedPair<IStatBuffer, float>(boost, boost.LifeTime));
            Injected?.Invoke(boost);
            return true;
        }

        public void Visit(IMovable movable)
        {
            if (_currentBoosters.Any(pair => pair.Key is IMovable == true))
                return;

            _isAllowBoost = true;
        }

        public void Visit(ICalculableScore calculableScore)
        {
            if (_currentBoosters.Any(pair => pair.Key is ICalculableScore == true))
                return;

            _isAllowBoost = true;
        }
    }
}