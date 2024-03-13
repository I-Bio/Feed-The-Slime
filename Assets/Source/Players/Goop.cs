using System;
using UnityEngine;

namespace Players
{
    public class Goop
    {
        private readonly float _scoreScaler;
        private readonly int _levelsPerStage;

        private SatietyStage _satiety;
        private float _maxScore;
        private float _score;
        private int _currentLevel;

        public Goop(float scoreScaler, int levelsPerStage)
        {
            _scoreScaler = scoreScaler;
            _levelsPerStage = levelsPerStage;
        }

        public event Action<float, float> ScoreChanged;
        public event Action SizeIncreased;
        public event Action LevelIncreased;
        public event Action Winning;

        public void IncreaseScore(float value)
        {
            _score += value;
            
            if (_score >= _maxScore)
                RaiseLevel();
            
            ScoreChanged?.Invoke(_score, _maxScore);
        }

        private void RaiseLevel()
        {
            _maxScore *= _scoreScaler;
            _currentLevel++;
            
            if (_currentLevel >= _levelsPerStage)
                RaiseStage();
        }

        private void RaiseStage()
        {
            LevelIncreased?.Invoke();
            int satiety = (int)_satiety++;
            
            if (Enum.GetValues(typeof(SatietyStage)).Length - 1 >= satiety)
            {
                _satiety = (SatietyStage)satiety;
                SizeIncreased?.Invoke();
            }
            else
                Winning?.Invoke();
        }
        
        
    }
}