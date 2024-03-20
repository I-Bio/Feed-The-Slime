﻿using System;
using Boosters;

namespace Players
{
    public class Goop : ISettable
    {
        private readonly float _scoreScaler;
        private readonly int _levelsPerStage;

        private ICalculableScore _calculableScore;
        private SatietyStage _stage;
        private float _maxScore;
        private float _score;
        private int _currentLevel;

        public Goop(SatietyStage stage,float scoreScaler, float maxScore, int levelsPerStage)
        {
            _stage = stage;
            _scoreScaler = scoreScaler;
            _maxScore = maxScore;
            _levelsPerStage = levelsPerStage;
        }

        public event Action<float, float> ScoreChanged;
        public event Action<SatietyStage> SizeIncreased;
        public event Action<int> LevelIncreased;
        public event Action Winning;

        public void SetBoost(IStatBuffer boost)
        {
            _calculableScore = boost as ICalculableScore;
        }
        
        public void IncreaseScore(float value)
        {
            _score += _calculableScore.CalculateScore(value);
            
            if (_score >= _maxScore)
                RaiseLevel();
            
            ScoreChanged?.Invoke(_score, _maxScore);
        }

        public void IncreaseScore()
        {
            ScoreChanged?.Invoke(_score, _maxScore);
        }

        private void RaiseLevel()
        {
            _maxScore *= _scoreScaler;
            _currentLevel++;
            LevelIncreased?.Invoke(_currentLevel);

            if (_currentLevel >= _levelsPerStage)
                RaiseStage();
        }

        private void RaiseStage()
        {
            int satiety = (int)_stage++;
            
            if (Enum.GetValues(typeof(SatietyStage)).Length - 1 >= satiety)
            {
                _stage = (SatietyStage)satiety;
                SizeIncreased?.Invoke(_stage);
            }
            else
                Winning?.Invoke();
        }
    }
}