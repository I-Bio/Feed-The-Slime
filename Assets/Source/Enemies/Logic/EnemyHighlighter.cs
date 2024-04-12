using Foods;
using UnityEngine;

namespace Enemies
{
    public class EnemyHighlighter : ObjectHighlighter
    {
        [SerializeField] private Color _color = Color.white;

        private bool _didColorChange;
        
        public override void Select(SatietyStage playerStage)
        {
            Outline.OutlineWidth = SelectValue;
            base.Select(playerStage);
        }
        
        public override void SetSelection()
        {
            base.SetSelection();
            
            if (_didColorChange == true)
                return;

            _didColorChange = true;
            Outline.OutlineColor = _color;
        }
    }
}