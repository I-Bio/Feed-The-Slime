using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Image))]
    public class ObjectFiller : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        
        private Image _image;

        public void Initialize()
        {
            _image = GetComponent<Image>();
        }
        
        public void EmptyUp(Action onFilled = null)
        {
            _image.DOFillAmount(_minValue, _duration).SetUpdate(true).OnComplete(() => onFilled?.Invoke());
        }
        
        public void FillUp(Action onFilled = null)
        {
            _image.DOFillAmount(_maxValue, _duration).SetUpdate(true).OnComplete(() => onFilled?.Invoke());
        }
    }
}