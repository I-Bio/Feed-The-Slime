using System;

namespace Foods
{
    public class Food
    {
        private readonly SatietyStage AccessStage;

        private bool _isAllowed;

        public Food(SatietyStage accessStage)
        {
            AccessStage = accessStage;
        }

        public event Action Highlighted;

        public event Action Allowed;

        public void CompareStage(SatietyStage stage)
        {
            if (stage < AccessStage)
                return;

            Highlighted?.Invoke();

            if (_isAllowed == true)
                return;

            _isAllowed = true;
            Allowed?.Invoke();
        }
    }
}