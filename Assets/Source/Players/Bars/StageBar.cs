using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class StageBar : MonoBehaviour
    {
        [SerializeField] private Slider[] _sliders;
        [SerializeField] private Color _pass;
        [SerializeField] private Image[] _stages;

        private int _pointer = -1;
        private int _maxScore;
        private int _levelsPerStage;
        private float _scaler;

        public void Initialize(int startMax, int levelsPerStage, float scaler)
        {
            _maxScore = startMax;
            _levelsPerStage = levelsPerStage;
            _scaler = scaler;
            
            Increase();
            _levelsPerStage++;
        }

        public void ChangeValue(float score)
        {
            if (_pointer >= _sliders.Length)
                return;
            
            _sliders[_pointer].value = score / _maxScore;
        }

        public void Increase()
        {
            for (int i = 1; i < _levelsPerStage; i++)
                _maxScore = Mathf.FloorToInt(_maxScore * _scaler);
            
            _pointer++;
            _stages[_pointer].color = _pass;
        }
    }
}