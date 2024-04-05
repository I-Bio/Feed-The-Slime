using System;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    public class StageBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Color _pass;
        [SerializeField] private Image[] _stages;

        private int _pointer;
        private int _maxScore;
        private int _maxStage;
        private int _levelStep;
        private int _levelsPerStage;

        public void Initialize(int startMax, int levelsPerStage, float scaler)
        {
            _maxScore = startMax;
            _levelsPerStage = levelsPerStage;
            _maxStage = Enum.GetValues(typeof(SatietyStage)).Length - 1;
            _levelStep = _levelsPerStage * _maxStage;

            for (int i = 1; i < _levelStep; i++)
                _maxScore = Mathf.FloorToInt(_maxScore * scaler);

            ChangeValue(_pointer);
            Increase();
        }

        public void DecreaseStep()
        {
            _levelStep--;
        }
        
        public void ChangeValue(float score)
        {
            _slider.value = score * _maxStage * _levelStep / _maxScore;
        }

        public void Increase()
        {
            _maxStage -= _pointer;
            _stages[_pointer].color = _pass;
            _pointer++;
        }
    }
}