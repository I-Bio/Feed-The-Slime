using System;
using System.Collections.Generic;
using System.Linq;

namespace Boosters
{
    public class BoosterService : IBoosterVisitor, IInsertable
    {
        private readonly List<IStatBuffer> _currentBoosters;
        private readonly List<IStatBuffer> _toDeleteBoosters;

        private bool _isAllowBoost;

        public BoosterService()
        {
            _currentBoosters = new List<IStatBuffer>();
            _toDeleteBoosters = new List<IStatBuffer>();
        }

        public event Action<IStatBuffer> Ejected; 
        public event Action<IStatBuffer> Injected; 

        public void Update(float delay)
        {
            foreach (IStatBuffer booster in _currentBoosters)
            {
                booster.LifeTime -= delay;
                
                if (booster.LifeTime <= 0f)
                    _toDeleteBoosters.Add(booster);
            }

            foreach (IStatBuffer booster in _toDeleteBoosters)
            {
                Ejected?.Invoke(booster);
                _currentBoosters.Remove(booster);
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
            _currentBoosters.Add(boost);
            Injected?.Invoke(boost);
            return true;
        }

        public void Visit(IMovable movable)
        {
            if (_currentBoosters.OfType<IMovable>().Any() == true)
                return;

            _isAllowBoost = true;
        }

        public void Visit(ICalculableScore calculableScore)
        {
            if (_currentBoosters.OfType<ICalculableScore>().Any() == true)
                return;

            _isAllowBoost = true;
        }
    }
}