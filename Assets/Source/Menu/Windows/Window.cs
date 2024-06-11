using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class Window : MonoBehaviour
    {
        private const float OffAlpha = 0f;
        private const float OnAlpha = 1f;
        
        [SerializeField] private bool _isPausing;
        [SerializeField] private bool _isReleasing;
        [SerializeField] private bool _have3dElements;
        [SerializeField] private GameObject[] _enablingObjects;
        
        private CanvasGroup _group;
        private Stopper _stopper;
        private GameObject _gameObject;

        public void Initialize(Stopper stopper)
        {
            _group = GetComponent<CanvasGroup>();
            _stopper = stopper;
            
            if (_have3dElements == true)
                _gameObject = gameObject;
        }

        public void Open()
        {
            if (_isPausing == true)
                _stopper.Pause();
            
            if (_have3dElements == true)
                _gameObject.SetActive(true);
            
            foreach(GameObject enabling in _enablingObjects)
                if (enabling != null)
                    enabling.SetActive(true);
            
            if (_group == null)
                return;
            
            _group.alpha = OnAlpha;
            _group.interactable = true;
            _group.blocksRaycasts = true;
        }

        public void Close()
        {
            if (_group == null)
                return;
            
            _group.alpha = OffAlpha;
            _group.interactable = false;
            _group.blocksRaycasts = false;
            
            foreach(GameObject disabling in _enablingObjects)
                if (disabling != null)
                    disabling.SetActive(false);
            
            if (_have3dElements == true)
                _gameObject.SetActive(false);
            
            if (_isReleasing == true)
                _stopper.Release();
        }
    }
}