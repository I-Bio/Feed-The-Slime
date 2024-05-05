using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Screen))]
    public class SubWindowSwitcher : MonoBehaviour
    {
        private Screen _screen;
        private Button[] _closeButtons;
        private Button[] _volumeButtons;
        private Window[] _parents;
        private List<Window> _toOpen;

        private void OnDestroy()
        {
            foreach (Button close in _closeButtons)
                close.onClick.RemoveListener(Hide);
            
            foreach (Button volume in _volumeButtons)
                volume.onClick.RemoveListener(ShowVolume);
        }

        public void Initialize(Stopper stopper, Button[] closeButtons, Button[] volumeButtons, Window[] parents)
        {
            _closeButtons = closeButtons;
            _volumeButtons = volumeButtons;
            _parents = parents;
            _toOpen = new List<Window>();
            _screen = GetComponent<Screen>();
            _screen.Initialize(stopper);

            foreach (Button close in _closeButtons)
                close.onClick.AddListener(Hide);
            
            foreach (Button volume in _volumeButtons)
                volume.onClick.AddListener(ShowVolume);
        }

        private void ChangeWindow(SubWindows window)
        {
            _screen.SetWindow((int)window);
            
            foreach (Window parent in _parents)
            {
                if (parent.IsActive == false)
                    continue;
                
                parent.TurnOff();
                _toOpen.Add(parent);
            }
        }

        private void Hide()
        {
            foreach (Window parent in _toOpen)
                parent.Open();

            _screen.Hide();
            _toOpen.Clear();
        }
        
        private void ShowVolume()
        {
            ChangeWindow(SubWindows.Pause);
        }
    }
}