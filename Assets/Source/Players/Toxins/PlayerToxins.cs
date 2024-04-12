using System;

namespace Players
{
    public class PlayerToxins
    {
        private readonly int _maxValue;
        private readonly int _minValue;
        
        private int _currentValue;

        public event Action GoingDie;
        public event Action<int> ToxinsChanged;

        public PlayerToxins(int maxValue, int minValue)
        {
            _maxValue = maxValue;
            _minValue = minValue;
            _currentValue = _minValue;
        }

        public void Increase()
        {
            if (_currentValue >= _maxValue)
                return;
            
            _currentValue++;
            CompareAccumulation();
        }

        public void Decrease()
        {
            if (_currentValue <= _minValue)
                return;
            
            _currentValue--;
            CompareAccumulation();
        }

        private void CompareAccumulation()
        {
            ToxinsChanged?.Invoke(_currentValue);
            
            if (_currentValue < _maxValue)
                return;
            
            GoingDie?.Invoke();
            _currentValue = _minValue;
            ToxinsChanged?.Invoke(_currentValue);
        }
    }
}