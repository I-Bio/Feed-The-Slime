using UnityEngine;

namespace Menu
{
    public class Screen : MonoBehaviour
    {
        [SerializeField] private Window[] _windows;

        private Window _current;

        public void Initialize(Stopper stopper)
        {
            foreach (Window window in _windows)
                window.Initialize(stopper);
        }

        public void SetWindow(int id)
        {
            if (_current != null)
                _current.Close();

            _current = _windows[id];
            _current.Open();
        }
    }
}