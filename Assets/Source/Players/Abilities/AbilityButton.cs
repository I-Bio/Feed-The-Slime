using System.Collections;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    [RequireComponent(typeof(LocalizedText))]
    public class AbilityButton : SpawnableObject
    {
        [SerializeField] private float _coolDownUpdateStep;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] protected float CoolDown;

        private LocalizedText _localized;
        
        public bool CanUse { get; private set; } = true;

        public void Initialize()
        {
            _localized = GetComponent<LocalizedText>();
            _text.SetText(_localized.Label);
        }

        public AbilityButton Use()
        {
            StartCoroutine(CoolDownRoutine());
            return this;
        }
        
        private IEnumerator CoolDownRoutine()
        {
            CanUse = false;
            _button.interactable = false;
            
            var wait = new WaitForSeconds(_coolDownUpdateStep);
            float time = 0f;
            
            while (time <= CoolDown)
            {
                yield return wait;
                time += _coolDownUpdateStep;
                _text.SetText(Mathf.FloorToInt(CoolDown - time).ToString());
            }
            
            _text.SetText(_localized.Label);
            CanUse = true;
            _button.interactable = true;
        }
    }
}