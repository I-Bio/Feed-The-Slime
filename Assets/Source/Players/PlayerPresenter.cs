using Boosters;

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
        
        public PlayerPresenter(Goop model, PlayerCollisionDetector collisionDetector, PlayerScanner scanner,
            SizeScaler sizeScaler, LevelBar levelBar, StageBar stageBar, IInsertable boosterService)
        {
            _model = model;
            _collisionDetector = collisionDetector;
            _scanner = scanner;
            _sizeScaler = sizeScaler;
            _levelBar = levelBar;
            _stageBar = stageBar;
            _boosterService = boosterService;
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
            _scanner.SetSatiety(stage);
            _sizeScaler.Scale(stage);
            _scanner.Rescan();
        }

        private void OnWinning()
        {
            
        }

        private void OnScoreGained(float value)
        {
            _model.IncreaseScore(value);
        }

        private void OnBoosterEntered(IBooster booster)
        {
            _boosterService.TryInsert(booster);
        }
    }
}