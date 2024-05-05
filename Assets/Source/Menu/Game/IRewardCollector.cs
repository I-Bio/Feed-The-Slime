using System;

namespace Menu
{
    public interface IRewardCollector
    {
        public event Action<int, bool, bool, Action> GoingCollect;
    }
}