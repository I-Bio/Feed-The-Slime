using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuReturner : MonoBehaviour
    {
        private Button _open;
        private Button _accept;
        private Button _decline;
        private IReturnSwitcher _switcher;
        
        private void OnDestroy()
        {
            _open.onClick.RemoveListener(_switcher.OpenWarning);
            _accept.onClick.RemoveListener(_switcher.AcceptWarning);
            _decline.onClick.RemoveListener(_switcher.DeclineWarning);
        }

        public void Initialize(Button open, Button accept, Button decline, IReturnSwitcher switcher)
        {
            _open = open;
            _accept = accept;
            _decline = decline;
            _switcher = switcher;

            _open.onClick.AddListener(_switcher.OpenWarning);
            _accept.onClick.AddListener(_switcher.AcceptWarning);
            _decline.onClick.AddListener(_switcher.DeclineWarning);
        }
    }
}