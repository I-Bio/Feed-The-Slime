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

        public Player(ICalculableScore calculableScore, SatietyStage stage, float scoreScaler, float startScore, float maxScore,
            int levelsPerStage)
        {
            Combiner = new StatCombiner<ICalculableScore>(new CombinedScore());
            Combiner.Add(calculableScore);
            _calculableScore = Combiner.GetRecombined();
            _stage = stage;
            ScoreScaler = scoreScaler;
            _score = startScore;
            _maxScore = maxScore;
            LevelsPerStage = levelsPerStage;
            _maxLevel = LevelsPerStage;
        }

        public event Action<SatietyStage, int, float, int> Loaded;
        public event Action<float, int> ScoreChanged;
        public event Action<SatietyStage> SizeIncreased;
        public event Action<int> LevelIncreased;
        public event Action Winning;
        
        public void Load()
        {
            if (_score <= (float)ValueConstants.Zero)
                return;

            float max = _maxScore;
            
            for (int i = 1; i < LevelsPerStage; i++)
                max = Mathf.FloorToInt(_maxScore * ScoreScaler);
            
            if (_score >= max)
            {
                _score = 0f;
                return;
            }

            float score = 0f;
            float maxScore = _maxScore;
            int level = 0;
            int maxLevel = _maxLevel;
            SatietyStage stage = _stage;
            float step = 0.1f;

            while (score < _score)
            {
                score += step;

                if (score < maxScore && Mathf.Approximately(score, maxScore) == false)
                    continue;
                
                maxScore = Mathf.FloorToInt(maxScore * ScoreScaler);
                level++;

                if (level < maxLevel)
                    continue;
                
                maxLevel += LevelsPerStage;
                stage++;
            }

            _maxScore = maxScore;
            _currentLevel = level;
            _maxLevel = maxLevel;
            _stage = stage;
            Loaded?.Invoke(_stage, _currentLevel, _score, (int)_maxScore);
        }
        
        public void SetBoost(ICalculableScore boost)
        {
            Combiner.ChangeBoost(boost);
            _calculableScore = Combiner.GetRecombined();
        }

        public void IncreaseScore(float value)
        {
            value = _calculableScore.CalculateScore(value);
            _score += value;

            while (_score >= _maxScore || Mathf.Approximately(_score, _maxScore))
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