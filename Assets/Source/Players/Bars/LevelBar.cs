using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class LevelBar : ObjectPool
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private Transform _container;
        [SerializeField] private string _slash;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _level;

        private Vector3 _popUpPosition;
        
        public void Initialize(float score, int maxScore)
        {
            ChangeScore(score, maxScore);
            _popUpPosition = _popUpPoint.position;
        }
        
        public void SetScore(float score, int maxScore, float value)
        {
            PullAndSetParent<PopUpText>(_popUpPosition, _container).Initialize(value);
            ChangeScore(score, maxScore);
        }

        public void SetLevel(int levelPoint)
        {
            _level.SetText(levelPoint.ToString());
        }

        private void ChangeScore(float score, int maxScore)
        {
            _slider.value = score / maxScore;
            _score.SetText($"{Mathf.FloorToInt(score)}{_slash}{maxScore}");
        }
    }
}