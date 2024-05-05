using DG.Tweening;
using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ObjectFlasher : MonoBehaviour
    {
        private const int Infinity = -1;
        
        [SerializeField] private float _duration;
        [SerializeField] private Vector3 _maxScale;
        [SerializeField, Range(0f, 1f)] private float _maxAlpha;
        
        private Transform _transform;
        private CanvasGroup _fade;
        
        private void Awake()
        {
            _fade = GetComponent<CanvasGroup>();
            _transform = transform;
            
            _transform.DOScale(_maxScale, _duration).SetLoops(Infinity, LoopType.Yoyo).SetUpdate(true);
            _fade.DOFade(_maxAlpha, _duration).SetLoops(Infinity, LoopType.Yoyo).SetUpdate(true);
        }
    }
}