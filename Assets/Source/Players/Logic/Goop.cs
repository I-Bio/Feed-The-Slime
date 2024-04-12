﻿using System;
using Boosters;
using UnityEngine;

namespace Players
{
    public class Goop : ISettable
    {
        private readonly float _scoreScaler;
        private readonly int _levelsPerStage;

        private ICalculableScore _calculableScore;
        private SatietyStage _stage;
        private int _maxScore;
        private float _score;
        private int _maxLevel;
        private int _currentLevel;

        public Goop(ICalculableScore calculableScore, SatietyStage stage, float scoreScaler, int maxScore,
            int levelsPerStage)
        {
            _calculableScore = calculableScore;
            _stage = stage;
            _scoreScaler = scoreScaler;
            _maxScore = maxScore;
            _levelsPerStage = levelsPerStage;
            _maxLevel = _levelsPerStage;
        }

        public event Action<float, int, float> ScoreChanged;
        public event Action<SatietyStage> SizeIncreased;
        public event Action<int> LevelIncreased;
        public event Action Winning;

        public void SetBoost(IStatBuffer boost)
        {
            _calculableScore = boost as ICalculableScore;
        }

        public void IncreaseScore(float value)
        {
            value = _calculableScore.CalculateScore(value);
            _score += value;

            if (_score >= _maxScore)
                RaiseLevel();

            ScoreChanged?.Invoke(_score, _maxScore, value);
        }

        private void RaiseLevel()
        {
            _maxScore = Mathf.FloorToInt(_maxScore * _scoreScaler);
            _currentLevel++;
            LevelIncreased?.Invoke(_currentLevel);

            if (_currentLevel >= _maxLevel)
                RaiseStage();
        }

        private void RaiseStage()
        {
            _maxLevel += _levelsPerStage;
            _stage++;
            SizeIncreased?.Invoke(_stage);
           
            if (Enum.GetValues(typeof(SatietyStage)).Length - 1 <= (int)_stage)
                Winning?.Invoke();
        }
    }
}