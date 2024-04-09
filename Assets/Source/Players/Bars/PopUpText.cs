using System;
using DG.Tweening;
using Spawners;
using TMPro;
using UnityEngine;

namespace Players
{
    public class PopUpText : SpawnableObject
    {
        [SerializeField] private string _plus;
        [SerializeField] private float _duration;
        [SerializeField] private TextMeshProUGUI _text;

        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(float value, Vector2 position)
        {
            _text.SetText($"{_plus}{value}");
            _rectTransform.DOAnchorPos(position, _duration).OnComplete(() =>
            {
                Push();
                _rectTransform.anchoredPosition = Vector2.zero;
            });
        }
    }
}