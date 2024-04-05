using Spawners;
using TMPro;
using UnityEngine;

namespace Players
{
    public class PopUpText : SpawnableObject
    {
        [SerializeField] private string _trigger;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Animator _animator;
        
        public void Initialize(float value)
        {
            _text.SetText(value.ToString());
            _animator.Play(_trigger);
        }
    }
}