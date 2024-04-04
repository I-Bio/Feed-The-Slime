using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ProgressionBar<T> : MonoBehaviour, IProgressionBar
    {
        
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _currentCount;
        [SerializeField] private string _maxText;
        [SerializeField] private string _slash = "/";
        [SerializeField] private PurchaseNames _name;
        [SerializeField] private Image _slider;
        [SerializeField] private SerializedPair<T>[] _purchases;
        
        private int _stage;
        private bool _isMax;

        public event Action<int, PurchaseNames, object> Bought;

        private string CurrentProgress => $"{_stage}{_slash}{_purchases.Length}";

        private void OnEnable()
        {
            _buyButton.onClick.AddListener(Buy);
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveListener(Buy);
        }

        public void Initialize(object value)
        {
            int id = 0;

            for (int i = 0; i < _purchases.Length - 1; i++)
                if (_purchases[i].Value.Equals(value))
                    id = i;
            
            Load(id);
        }
        
        public void CompareCrystals(int crystalsCount)
        {
            if (_isMax == true)
            {
                if (_buyButton.interactable == true)
                    _buyButton.interactable = false;
                
                return;
            }
            
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

        private void Buy()
        {
            Bought?.Invoke(_purchases[_stage].Key, _name, _purchases[_stage].Value);
            _slider.fillAmount = _stage / _purchases.Length;
            _stage++;
            _price.SetText(_stage < _purchases.Length ? _purchases[_stage].ToString() : _maxText);
            _currentCount.SetText(_stage < _purchases.Length ? CurrentProgress : _maxText);
            
            if (_purchases.Length != _stage) 
                return;
            
            _isMax = true;
        }

        private void Load(int id)
        {
            _stage = id;
            _slider.fillAmount = _stage / _purchases.Length;
            
            _price.SetText(_purchases[_stage].Key.ToString());
            _currentCount.SetText(CurrentProgress);
        }
    }
}