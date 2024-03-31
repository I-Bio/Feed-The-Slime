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
        private readonly PlayerAnimation _animation;
        private readonly AbilityCaster _caster;
        private readonly EffectReproducer _effectReproducer;
        private readonly SoundReproducer _soundReproducer;
        private readonly IInsertable _boosterService;
        private readonly IMover _mover;

        public PlayerPresenter(Goop model, PlayerCollisionDetector collisionDetector, PlayerScanner scanner,
            SizeScaler sizeScaler, LevelBar levelBar, StageBar stageBar, IInsertable boosterService,
            PlayerAnimation animation, AbilityCaster caster, IMover mover, EffectReproducer effectReproducer,
            SoundReproducer soundReproducer)
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
            _mover = mover;
            _effectReproducer = effectReproducer;
            _soundReproducer = soundReproducer;
        }

        public void Enable()
        {
            _model.ScoreChanged += OnScoreChanged;
            _model.LevelIncreased += OnLevelIncreased;
            _model.SizeIncreased += OnSizeIncreased;
            _model.Winning += OnWinning;

            _collisionDetector.ScoreGained += OnScoreGained;
            _collisionDetector.BoosterEntered += OnBoosterEntered;
            _caster.Hid += OnHid;
            _caster.Showed += OnShowed;
            _caster.SpitCasted += OnSpitCasted;
        }

        public void Disable()
        {
            _model.ScoreChanged -= OnScoreChanged;
            _model.LevelIncreased -= OnLevelIncreased;
            _model.SizeIncreased -= OnSizeIncreased;
            _model.Winning -= OnWinning;

            _collisionDetector.ScoreGained -= OnScoreGained;
            _collisionDetector.BoosterEntered -= OnBoosterEntered;
            _caster.Hid -= OnHid;
            _caster.Showed -= OnShowed;
            _caster.SpitCasted -= OnSpitCasted;
        }

        private void OnScoreChanged(float score, float maxScore)
        {
            _soundReproducer.PlayClip(SoundType.ScoreGain);
            _levelBar.SetScore(score, maxScore);
        }

        private void OnLevelIncreased(int level)
        {
            _soundReproducer.PlayClip(SoundType.LevelUp);
            _levelBar.SetLevel(level);
        }

        private void OnSizeIncreased(SatietyStage stage)
        {
            _soundReproducer.PlayClip(SoundType.StageUp);
            _effectReproducer.PlayEffect(EffectType.StageUp);
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

        private void OnHid()
        {
            _mover.ProhibitMove();
            _animation.PlayHide();
        }

        private void OnShowed()
        {
            _mover.AllowMove();
            _animation.PlayIdle();
        }

        private void OnSpitCasted()
        {
            _soundReproducer.PlayClip(SoundType.Spit);
            _animation.PlayAttack();
        }
    }
}