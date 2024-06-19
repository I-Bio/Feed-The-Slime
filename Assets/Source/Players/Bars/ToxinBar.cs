using System;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class ToxinBar : MonoBehaviour
    {
        [SerializeField] private GameObject _holder;
        [SerializeField] private Slider _slider;

        private float _maxValue;
        private int _minValue;

        public event Action Hid;

        public void Initialize(float maxValue, int minValue)
        {
            _maxValue = maxValue;
            _minValue = minValue;

            ChangeValue(_minValue);
        }

        public void ChangeValue(int value)
        {
            _slider.value = value / _maxValue;

            if (value > _minValue && _holder.activeSelf == false)
                Show();

            if (value <= _minValue && _holder.activeSelf == true)
                Hide();
        }

        private void Show()
        {
            _holder.SetActive(true);
        }

        private void Hide()
        {
            _holder.SetActive(false);
            Hid?.Invoke();
        }
    }
}