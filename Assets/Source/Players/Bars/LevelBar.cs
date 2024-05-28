using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    [RequireComponent(typeof(LocalizedText))]
    public class LevelBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private string _slash;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _level;

        private LocalizedText _localized;
        
        public void Initialize(float score, int maxScore)
        {
            _localized = GetComponent<LocalizedText>();
            ChangeScore(score, maxScore);
            SetLevel((int)score);
        }
        
        public void SetLevel(int levelPoint)
        {
            _level.SetText($"{_localized.Label} {levelPoint}");
        }

        public void ChangeScore(float score, int maxScore)
        {
            _slider.value = score / maxScore;
            _score.SetText($"{score:F1}{_slash}{maxScore:F1}");
        }
    }
}