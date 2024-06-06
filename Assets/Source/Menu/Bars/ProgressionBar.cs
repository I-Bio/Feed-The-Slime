using System;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressionBar<T> : LeanLocalizedBehaviour, IProgressionBar
    {
        
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _currentCount;
        [SerializeField] private string _maxText;
        [SerializeField] private string _slash = "/";
        [SerializeField] private PurchaseNames _name;
        [SerializeField] private Image _slider;
        [SerializeField] private SerializedPair<int, T>[] _purchases;
        
        private int _stage;
        private bool _isMax;

        public event Action<int, PurchaseNames, object> Bought;

        private new void OnEnable()
        {
            _buyButton.onClick.AddListener(Buy);
            base.OnEnable();
        }

        private new void OnDisable()
        {
            _buyButton.onClick.RemoveListener(Buy);
            base.OnDisable();
        }

        public void Initialize(object value)
        {
            int id = 0;

            for (int i = 1; i <= _purchases.Length; i++)
                if (_purchases[i - 1].Value.Equals(value))
                    id = i;
            
            Load(id);
        }
        
        public void CompareCrystals(int crystalsCount)
        {
            if (_isMax == true)
                return;
            
            if (crystalsCount >= _purchases[_stage].Key)
            {
                if (_buyButton.interactable == true)
                    return;

                _buyButton.interactable = true;
                return;
            }
            
            if (_buyButton.interactable == false)
                return;

            _buyButton.interactable = false;
        }
        
        public override void UpdateTranslation(LeanTranslation translation)
        {
            if (translation == null)
                return;
            
            if (translation.Data is string == false)
                return;
            
            _maxText = translation.Data as string;
            Display();
        }

        private void Buy()
        {
            _stage++;
            DeactivateBuyButton();
            Bought?.Invoke(_purchases[_stage - 1].Key, _name, _purchases[_stage - 1].Value);
            Display();
        }

        private void Load(int id)
        {
            _stage = id;
            DeactivateBuyButton();
            Display();
        }

        private void Display()
        {
            _slider.fillAmount = _stage / (float)_purchases.Length;
            _price.SetText(_stage < _purchases.Length ? _purchases[_stage].Key.ToString() : string.Empty);
            _currentCount.SetText(_stage < _purchases.Length ? $"{_stage}{_slash}{_purchases.Length}" : _maxText);
        }

        private void DeactivateBuyButton()
        {
            if (_purchases.Length != _stage) 
                return;
            
            _isMax = true;
            _buyButton.interactable = false;
        }
    }
}