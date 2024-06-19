using System;

namespace Menu
{
    public interface IProgressionBar
    {
        public event Action<int, PurchaseNames, object> Bought;

        public void Initialize(object value);

        public void CompareCrystals(int crystalsCount);
    }
}