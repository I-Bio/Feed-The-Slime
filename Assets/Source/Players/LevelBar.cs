using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class LevelBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _currentScore;
        [SerializeField] private TextMeshProUGUI _maxScore;
        [SerializeField] private TextMeshProUGUI _level;

        public void SetScore(float score, float maxScore)
        {
            _slider.value = score / maxScore;
            _currentScore.SetText(Mathf.CeilToInt(score).ToString());
            _maxScore.SetText(Mathf.CeilToInt(maxScore).ToString());
        }

        public void SetLevel(int levelPoint)
        {
            _level.SetText(levelPoint.ToString());
        }
    }
}