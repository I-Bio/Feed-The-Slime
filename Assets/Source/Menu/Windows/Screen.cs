using UnityEngine;

namespace Menu
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] private GameObject[] _windows;

        private int _currentActiveId;
        
        public void SetWindow(int id)
        {
            _windows[_currentActiveId].SetActive(false);
            _windows[id].SetActive(true);
            _currentActiveId = id;
        }
        
        public void HideWindow(HideOption option)
        {
            if (option == HideOption.Main)
                SetLast();
            else if (option == HideOption.HideAll)
                Hide();
        }

        private void SetLast()
        {
            _windows[_currentActiveId].SetActive(false);
            _windows[^1].SetActive(true);
            _currentActiveId = _windows.Length - 1;
        }

        private void Hide()
        {
            _windows[_currentActiveId].SetActive(false);
        }
    }
}