using System;

namespace Foods
{
    public class Food
    {
        private readonly SatietyStage _accessStage;

        private bool _isAllowed;
        
        public Food(SatietyStage accessStage)
        {
            _accessStage = accessStage;
        }

        public event Action Highlighted;
        public event Action Allowed;

        public void CompareStage(SatietyStage stage)
        {
            if (stage < _accessStage)
                return;
            
            Highlighted?.Invoke();
            
            if (_isAllowed == true)
                return;
            
            _isAllowed = true;
            Allowed?.Invoke();
        }
    }
}