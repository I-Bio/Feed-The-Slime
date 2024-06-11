using System;
using QuickOutline;
using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(Outline))]
    public class ObjectHighlighter : MonoBehaviour, ISelectable
    {
        public Outline Outline;
        
        private Transform _transform;
        private float _deselectValue;
        private float _selectValue;
        private bool _isSelect;
        private bool _didInitialize;
        
        public event Action<SatietyStage> GoingSelect;

        public Vector3 Position => _transform.position;
        
        public void Initialize(float deselectValue, float selectValue, Transform transform)
        {
            if (_didInitialize == true)
                return;

            Outline = GetComponent<Outline>();
            _deselectValue = deselectValue;
            _selectValue = selectValue;
            _transform = transform;
            Outline.Initialize();
            _isSelect = true;
            Deselect();
            _didInitialize = true;
        }

        public void Select(SatietyStage playerStage)
        {
            OnSelecting();
            GoingSelect?.Invoke(playerStage);
        }

        public void SetSelection()
        {
            Highlight();
            OnSetSelection();
        }

        public void Highlight()
        {
            if (_isSelect == true)
                return;
            
            _isSelect = true;
            Outline.OutlineWidth = _selectValue;
        }

        public void Deselect()
        {
            if (_isSelect == false)
                return;
            
            _isSelect = false;
            Outline.OutlineWidth = _deselectValue;
        }

        public virtual void OnSelecting() {}
        public virtual void OnSetSelection() {}
    }
}