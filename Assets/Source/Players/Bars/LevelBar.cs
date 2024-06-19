using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class LevelBar : LeanLocalizedBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private string _slash;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _level;

        private string _localized;
        private int _lastPoint;

        public override void UpdateTranslation(LeanTranslation translation)
        {
            if (translation == null)
                return;

            if (translation.Data is string == false)
                return;

            _localized = translation.Data as string;
            SetLevel(_lastPoint);
        }

        public void Initialize(float score, int maxScore)
        {
            ChangeScore(score, maxScore);
            SetLevel((int)score);
        }

        public void SetLevel(int levelPoint)
        {
            _level.SetText($"{_localized} {levelPoint}");
            _lastPoint = levelPoint;
        }

        public void ChangeScore(float score, int maxScore)
        {
            _slider.value = score / maxScore;
            _score.SetText($"{score:F1}{_slash}{maxScore:F1}");
        }
    }
}