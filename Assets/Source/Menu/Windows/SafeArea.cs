using UnityEngine;

namespace Menu
{
    public class SafeArea : MonoBehaviour
    {
        private void Awake()
        {
            UpdateScreen();
        }

        private void UpdateScreen()
        {
            RectTransform transform = GetComponent<RectTransform>();
            Rect safeArea = UnityEngine.Screen.safeArea;
            int height = UnityEngine.Screen.height;

            transform.offsetMin = new Vector2(safeArea.xMin, safeArea.yMin);
            transform.offsetMax = new Vector2(safeArea.xMax - safeArea.width, (safeArea.yMax + safeArea.yMax - safeArea.height) - height);
        }
    }
}