using System;
using Boosters;
using UnityEngine;

namespace Players
{
    public class Player : ISettable<ICalculableScore>
    {
        private readonly float ScoreScaler;
        private readonly int LevelsPerStage;
        private readonly StatCombiner<ICalculableScore> Combiner;

        private ICalculableScore _calculableScore;
        private SatietyStage _stage;
        private float _maxScore;
        private float _score;
        private int _maxLevel;
        private int _currentLevel;

        public Player(ICalculableScore calculableScore, SatietyStage stage, float scoreScaler, float maxScore,
            int levelsPerStage)
        {
            Combiner = new StatCombiner<ICalculableScore>(new CombinedScore());
            Combiner.Add(calculableScore);
            _calculableScore = Combiner.GetRecombined();
            _stage = stage;
            ScoreScaler = scoreScaler;
            _maxScore = maxScore;
            LevelsPerStage = levelsPerStage;
            _maxLevel = LevelsPerStage;
        }

        public event Action<float, int> ScoreChanged;
        public event Action<SatietyStage> SizeIncreased;
        public event Action<int> LevelIncreased;
        public event Action Winning;

        public void SetBoost(ICalculableScore boost)
        {
            Combiner.ChangeBoost(boost);
            _calculableScore = Combiner.GetRecombined();
        }

        public void IncreaseScore(float value)
        {
            value = _calculableScore.CalculateScore(value);
            _score += value;
            
            if (_score >= _maxScore || Mathf.Approximately(_score, _maxScore))
                RaiseLevel();

            ScoreChanged?.Invoke(_score, (int)_maxScore);
        }

        private void RaiseLevel()
        {
            _maxScore = Mathf.FloorToInt(_maxScore * ScoreScaler);
            _currentLevel++;
            LevelIncreased?.Invoke(_currentLevel);

            if (_currentLevel >= _maxLevel)
                RaiseStage();
        }

        private void RaiseStage()
        {
            _maxLevel += LevelsPerStage;
            _stage++;
            SizeIncreased?.Invoke(_stage);
           
            if (Enum.GetValues(typeof(SatietyStage)).Length - 1 <= (int)_stage)
                Winning?.Invoke();
        }
    }
}