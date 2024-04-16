using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class PlayerScanner : MonoBehaviour
    {
        private readonly List<ISelectable> _selectables = new();

        private Coroutine _routine;
        private SatietyStage _stage;
        
        public void SetStage(SatietyStage stage)
        {
            _stage = stage;
        }

        public void Rescan()
        {
            foreach (ISelectable selectable in _selectables)
                selectable.Select(_stage);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ISelectable selectable) == false)
                return;
            
            _selectables.Add(selectable);
            selectable.Select(_stage);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ISelectable selectable) == false)
                return;

            _selectables.Remove(selectable);
            selectable.Deselect();
        }
    }
}