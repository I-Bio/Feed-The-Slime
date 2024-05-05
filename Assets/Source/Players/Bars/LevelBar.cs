using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    [RequireComponent(typeof(LocalizedText))]
    public class LevelBar : ObjectPool
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private RectTransform _targetPoint;
        [SerializeField] private Transform _container;
        [SerializeField] private string _slash;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _level;

        private LocalizedText _localized;
        private Vector2 _targetPosition;
        
        public void Initialize(float score, int maxScore)
        {
            _localized = GetComponent<LocalizedText>();
            ChangeScore(score, maxScore);
            SetLevel((int)score);
            _targetPosition = _targetPoint.anchoredPosition;
        }
        
        public void SetScore(float score, int maxScore, float value)
        {
            Pull<PopUpText>(_container).Initialize(value, _targetPosition);
            ChangeScore(score, maxScore);
        }

        public void SetLevel(int levelPoint)
        {
            _level.SetText($"{_localized.Label} {levelPoint}");
        }

        private void ChangeScore(float score, int maxScore)
        {
            _slider.value = score / maxScore;
            _score.SetText($"{Mathf.FloorToInt(score)}{_slash}{maxScore}");
        }
    }
}