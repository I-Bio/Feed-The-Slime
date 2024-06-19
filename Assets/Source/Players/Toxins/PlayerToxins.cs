using System;

namespace Players
{
    public class PlayerToxins
    {
        private readonly int MaxValue;
        private readonly int MinValue;

        private int _currentValue;

        public PlayerToxins(int maxValue, int minValue)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            _currentValue = MinValue;
        }

        public event Action GoingDie;

        public event Action<int> Changed;

        public void Increase()
        {
            if (_currentValue >= MaxValue)
                return;

            _currentValue++;
            CompareAccumulation();
        }

        public void Decrease()
        {
            if (_currentValue <= MinValue)
                return;

            _currentValue--;
            CompareAccumulation();
        }

        public void Drop()
        {
            _currentValue = (int)ValueConstants.Zero;
            CompareAccumulation();
        }

        private void CompareAccumulation()
        {
            Changed?.Invoke(_currentValue);

            if (_currentValue < MaxValue)
                return;

            GoingDie?.Invoke();
            _currentValue = MinValue;
            Changed?.Invoke(_currentValue);
        }
    }
}