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
        [SerializeField] private float _coolDown;

        private LocalizedText _localized;
        private WaitForSeconds _wait;

        public bool CanUse { get; private set; } = true;

        public void Initialize(float coolDown = float.NaN)
        {
            _localized = GetComponent<LocalizedText>();
            _text.SetText(_localized.Label);

            _wait ??= new WaitForSeconds(_coolDownUpdateStep);

            if (float.IsNaN(coolDown) == false)
                _coolDown = coolDown;
        }

        public AbilityButton Activate()
        {
            StartCoroutine(CoolDownRoutine());
            return this;
        }

        private IEnumerator CoolDownRoutine()
        {
            CanUse = false;
            _button.interactable = false;

            float time = 0f;

            while (time <= _coolDown)
            {
                yield return _wait;
                time += _coolDownUpdateStep;
                _text.SetText(Mathf.FloorToInt(_coolDown - time).ToString());
            }

            _text.SetText(_localized.Label);
            CanUse = true;
            _button.interactable = true;
        }
    }
}