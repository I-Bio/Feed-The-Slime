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
        
        public void Hide()
        {
            _windows[_currentActiveId].SetActive(false);
        }
    }
}