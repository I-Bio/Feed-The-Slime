using Foods;
using UnityEngine;

namespace Enemies
{
    public class EnemyHighlighter : ObjectHighlighter
    {
        private readonly Color Color = Color.white;

        private bool _didColorChange;

        public override void OnSelecting()
        {
            Highlight();
        }

        public override void OnSetSelection()
        {
            if (_didColorChange == true)
                return;

            _didColorChange = true;
            SetColor(Color);
        }
    }
}