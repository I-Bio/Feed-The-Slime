using System;
using UnityEngine;

namespace Foods
{
    public class ObjectHighlighter : MonoBehaviour, ISelectable
    {
        private MeshRenderer _meshRenderer;
        private Material _standard;
        private Material _highlighted;
        private bool _isSelect;
        
        public void Initialize(MeshRenderer meshRenderer, Material standard, Material highlighted)
        {
            _meshRenderer = meshRenderer;
            _standard = standard;
            _highlighted = highlighted;
        }

        public event Action<SatietyStage> GoingSelect;

        public void Select(SatietyStage playerStage)
        {
            if (_isSelect == true)
                return;
            
            GoingSelect?.Invoke(playerStage);
        }

        public void SetSelection()
        {
            _isSelect = true;
            _meshRenderer.material = _highlighted;
        }

        public void Deselect()
        {
            _isSelect = false;
            _meshRenderer.material = _standard;
        }
    }
}