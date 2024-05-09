using System;
using System.Collections.Generic;
using System.Linq;

namespace Boosters
{
    public class BoosterService : IBoosterVisitor, IUsable
    {
        private readonly List<SerializedPair<IStat, float>> _currentBoosters = new();
        private readonly List<SerializedPair<IStat, float>> _toDeleteBoosters = new();

        private bool _isAllowBoost;

        public event Action<IStat> Ejected; 
        public event Action<IStat> Injected; 

        public void Update(float delay)
        {
            for (int i = 0; i < _currentBoosters.Count; i++)
            {
                var unpacked = _currentBoosters[i];
                unpacked.Value -= delay;
                _currentBoosters[i] = unpacked;
                
                if (_currentBoosters[i].Value <= 0f)
                    _toDeleteBoosters.Add(_currentBoosters[i]);
            }

            foreach (SerializedPair<IStat, float> pair in _toDeleteBoosters)
            {
                Ejected?.Invoke(pair.Key);
                _currentBoosters.Remove(pair);
            }
            
            _toDeleteBoosters.Clear();
        }

        public bool TryInsert(IBooster booster)
        {
            IStat boost = booster.GetBoost();
            
            boost.Accept(this);

            if (_isAllowBoost == false)
                return false;
            
            _isAllowBoost = false;
            
            booster.Use();
            _currentBoosters.Add(new SerializedPair<IStat, float>(boost, boost.LifeTime));
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