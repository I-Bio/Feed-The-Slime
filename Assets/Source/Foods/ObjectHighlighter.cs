using System;
using QuickOutline;
using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(Outline))]
    public class ObjectHighlighter : MonoBehaviour, ISelectable
    {
        private Renderer _renderer;
        private Material[] _standard;
        private Material[] _highlighted;
        private bool _isSelect;
        
        public void Initialize(Renderer renderer, Material[] standard, Material[] highlighted)
        {
            _renderer = renderer;
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
            _renderer.materials = _highlighted;
        }

        public void Deselect()
        {
            _isSelect = false;
            _renderer.materials = _standard;
        }
    }
}