using System;
using QuickOutline;
using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(Outline))]
    public class ObjectHighlighter : MonoBehaviour, ISelectable
    {
        private float _deselectValue;
        private bool _isSelect;
        
        protected Outline Outline;
        protected float SelectValue;
        
        public void Initialize(float deselectValue, float selectValue)
        {
            Outline = GetComponent<Outline>();
            _deselectValue = deselectValue;
            SelectValue = selectValue;
            Outline.Initialize();
        }

        public event Action<SatietyStage> GoingSelect;
        
        public virtual void Select(SatietyStage playerStage)
        {
            if (_isSelect == true)
                return;
            
            GoingSelect?.Invoke(playerStage);
        }

        public virtual void SetSelection()
        {
            _isSelect = true;
            Outline.OutlineWidth = SelectValue;
        }

        public void Deselect()
        {
            _isSelect = false;
            Outline.OutlineWidth = _deselectValue;
        }
    }
}