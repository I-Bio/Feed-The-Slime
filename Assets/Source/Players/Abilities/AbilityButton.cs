using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private float _coolDown;
        [SerializeField] private float _coolDownUpdateStep;
        [SerializeField] private string _defaultName;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        public bool CanUse { get; private set; } = true;

        public void Use()
        {
            StartCoroutine(CoolDownRoutine());
        }
        
        private IEnumerator CoolDownRoutine()
        {
            CanUse = false;
            _button.interactable = false;
            
            var wait = new WaitForSeconds(_coolDownUpdateStep);
            float time = 0f;
            
            while (time <= _coolDown)
            {
                yield return wait;
                time += _coolDownUpdateStep;
                _text.SetText(Mathf.FloorToInt(_coolDown - time).ToString());
            }
            
            _text.SetText(_defaultName);
            CanUse = true;
            _button.interactable = true;
        }
    }
}