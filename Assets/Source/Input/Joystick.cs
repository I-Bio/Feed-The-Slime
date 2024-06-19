using System;
using UnityEngine;

namespace Input
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private RectTransform _holderTransform;
        [SerializeField] private CanvasGroup _fade;

        public event Action Released;

        public void CalculatePosition(Vector2 position)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_parent, position, _camera) == false)
                return;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _parent,
                    position,
                    _camera,
                    out Vector2 localPosition) == false)
                return;

            _holderTransform.anchoredPosition = localPosition;
        }

        public void Activate(bool canShow = true)
        {
            if (canShow == true)
                _fade.alpha = 1f;
        }

        public void Release()
        {
            _fade.alpha = 0f;
            Released?.Invoke();
        }
    }
}