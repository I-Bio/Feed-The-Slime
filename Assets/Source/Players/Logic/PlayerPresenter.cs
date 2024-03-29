﻿using Boosters;

namespace Players
{
    public class PlayerPresenter : IPresenter
    {
        private readonly Goop _model;
        private readonly PlayerCollisionDetector _collisionDetector;
        private readonly PlayerScanner _scanner;
        private readonly SizeScaler _sizeScaler;
        private readonly LevelBar _levelBar;
        private readonly StageBar _stageBar;
        private readonly IInsertable _boosterService;
        private readonly PlayerAnimation _animation;
        private readonly IStageSettable _caster;
        
        public PlayerPresenter(Goop model, PlayerCollisionDetector collisionDetector, PlayerScanner scanner,
            SizeScaler sizeScaler, LevelBar levelBar, StageBar stageBar, IInsertable boosterService, PlayerAnimation animation, IStageSettable caster)
        {
            _model = model;
            _collisionDetector = collisionDetector;
            _scanner = scanner;
            _sizeScaler = sizeScaler;
            _levelBar = levelBar;
            _stageBar = stageBar;
            _boosterService = boosterService;
            _animation = animation;
            _caster = caster;
        }
        
        public void Enable()
        {
            _model.ScoreChanged += OnScoreChanged;
            _model.LevelIncreased += OnLevelIncreased;
            _model.SizeIncreased += OnSizeIncreased;
            _model.Winning += OnWinning;

            _collisionDetector.ScoreGained += OnScoreGained;
            _collisionDetector.BoosterEntered += OnBoosterEntered;
        }

        public void Disable()
        {
            _model.ScoreChanged -= OnScoreChanged;
            _model.LevelIncreased -= OnLevelIncreased;
            _model.SizeIncreased -= OnSizeIncreased;
            _model.Winning -= OnWinning;
            
            _collisionDetector.ScoreGained -= OnScoreGained;
            _collisionDetector.BoosterEntered -= OnBoosterEntered;
        }

        private void OnScoreChanged(float score, float maxScore)
        {
            _levelBar.SetScore(score, maxScore);
        }

        private void OnLevelIncreased(int level)
        {
            _levelBar.SetLevel(level);
        }

        private void OnSizeIncreased(SatietyStage stage)
        {
            _caster.SetStage(stage);
            _scanner.SetStage(stage);
            _scanner.Rescan();
            _sizeScaler.Scale(stage);
        }

        private void OnWinning()
        {
            
        }

        private void OnScoreGained(float value)
        {
            _animation.PlayAttack();
            _model.IncreaseScore(value);
        }

        private void OnBoosterEntered(IBooster booster)
        {
            _boosterService.TryInsert(booster);
        }
    }
}