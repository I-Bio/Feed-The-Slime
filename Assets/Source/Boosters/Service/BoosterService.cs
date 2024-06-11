using System;
using System.Collections.Generic;
using System.Linq;

namespace Boosters
{
    public class BoosterService : IBoosterVisitor, IUsable
    {
        private readonly List<SerializedPair<IStat, float>> CurrentBoosters = new();
        private readonly List<SerializedPair<IStat, float>> ToDeleteBoosters = new();

        private bool _isAllowBoost;

        public event Action<IStat> Ejected; 
        public event Action<IStat> Injected; 

        public void Update(float delay)
        {
            for (int i = 0; i < CurrentBoosters.Count; i++)
            {
                SerializedPair<IStat, float> unpacked = CurrentBoosters[i];
                unpacked.Value -= delay;
                CurrentBoosters[i] = unpacked;
                
                if (CurrentBoosters[i].Value <= 0f)
                    ToDeleteBoosters.Add(CurrentBoosters[i]);
            }

            foreach (SerializedPair<IStat, float> pair in ToDeleteBoosters)
            {
                Ejected?.Invoke(pair.Key);
                CurrentBoosters.Remove(pair);
            }
            
            ToDeleteBoosters.Clear();
        }

        public bool TryInsert(IBooster booster)
        {
            IStat boost = booster.GetBoost();
            
            boost.Accept(this);

            if (_isAllowBoost == false)
                return false;
            
            _isAllowBoost = false;
            
            booster.Use();
            CurrentBoosters.Add(new SerializedPair<IStat, float>(boost, boost.LifeTime));
            Injected?.Invoke(boost);
            return true;
        }

        public void Visit(IMovable movable)
        {
            if (CurrentBoosters.Any(pair => pair.Key is IMovable == true))
                return;

            _isAllowBoost = true;
        }

        public void Visit(ICalculableScore calculableScore)
        {
            if (CurrentBoosters.Any(pair => pair.Key is ICalculableScore == true))
                return;

            _isAllowBoost = true;
        }
    }
}