using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Screen))]
    public class WindowSwitcher : MonoBehaviour
    {
        [SerializeField] private Button _evolution;
        [SerializeField] private Button _leaders;
        [SerializeField] private Button[] _closeButtons;
        [SerializeField] private HideOption _option;

        private Screen _screen;

        private void Awake()
        {
            _screen = GetComponent<Screen>();
        }

        private void OnEnable()
        {
            _evolution.onClick.AddListener(ShowEvolution);
            
            foreach (Button button in _closeButtons)
                button.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _evolution.onClick.RemoveListener(ShowEvolution);
            
            foreach (Button button in _closeButtons)
                button.onClick.RemoveListener(Hide);
        }
        
        public void Hide()
        {
            _screen.HideWindow(_option);
        }

        private void ShowEvolution()
        {
            _screen.SetWindow((int)MainMenuWindows.Evolution);
        }
    }
}