using Boosters;
using Menu;

namespace Players
{
    public class PlayerPresenter : IPresenter
    {
        private readonly Goop Model;
        private readonly PlayerCollisionDetector CollisionDetector;
        private readonly PlayerScanner Scanner;
        private readonly SizeScaler SizeScaler;
        private readonly LevelBar LevelBar;
        private readonly StageBar StageBar;
        private readonly PlayerAnimation Animation;
        private readonly AbilityCaster Caster;
        private readonly EffectReproducer EffectReproducer;
        private readonly SoundReproducer SoundReproducer;
        private readonly IInsertable BoosterService;
        private readonly IMover Mover;
        private readonly IGame Game;

        public PlayerPresenter(Goop model, PlayerCollisionDetector collisionDetector, PlayerScanner scanner,
            SizeScaler sizeScaler, LevelBar levelBar, StageBar stageBar, IInsertable boosterService,
            PlayerAnimation animation, AbilityCaster caster, IMover mover, EffectReproducer effectReproducer,
            SoundReproducer soundReproducer, IGame game)
        {
            Model = model;
            CollisionDetector = collisionDetector;
            Scanner = scanner;
            SizeScaler = sizeScaler;
            LevelBar = levelBar;
            StageBar = stageBar;
            BoosterService = boosterService;
            Animation = animation;
            Caster = caster;
            Mover = mover;
            EffectReproducer = effectReproducer;
            SoundReproducer = soundReproducer;
            Game = game;
        }

        public void Enable()
        {
            Model.ScoreChanged += OnScoreChanged;
            Model.LevelIncreased += OnLevelIncreased;
            Model.SizeIncreased += OnSizeIncreased;
            Model.Winning += OnWinning;

            CollisionDetector.ScoreGained += OnScoreGained;
            CollisionDetector.BoosterEntered += OnBoosterEntered;
            CollisionDetector.EnemyContacted += OnEnemyContacted;
            Caster.Hid += OnHid;
            Caster.Showed += OnShowed;
            Caster.SpitCasted += OnSpitCasted;
        }

        public void Disable()
        {
            Model.ScoreChanged -= OnScoreChanged;
            Model.LevelIncreased -= OnLevelIncreased;
            Model.SizeIncreased -= OnSizeIncreased;
            Model.Winning -= OnWinning;

            CollisionDetector.ScoreGained -= OnScoreGained;
            CollisionDetector.BoosterEntered -= OnBoosterEntered;
            CollisionDetector.EnemyContacted -= OnEnemyContacted;
            Caster.Hid -= OnHid;
            Caster.Showed -= OnShowed;
            Caster.SpitCasted -= OnSpitCasted;
        }

        private void OnScoreChanged(float score, int maxScore, float value)
        {
            SoundReproducer.PlayClip(SoundType.ScoreGain);
            LevelBar.SetScore(score, maxScore, value);
            StageBar.ChangeValue(score);
        }

        private void OnLevelIncreased(int level)
        {
            SoundReproducer.PlayClip(SoundType.LevelUp);
            LevelBar.SetLevel(level);
        }

        private void OnSizeIncreased(SatietyStage stage)
        {
            SoundReproducer.PlayClip(SoundType.StageUp);
            EffectReproducer.PlayEffect(EffectType.StageUp);
            Caster.SetStage(stage);
            Scanner.SetStage(stage);
            CollisionDetector.SetStage(stage);
            Game.SetStage(stage);
            StageBar.Increase();
            Scanner.Rescan();
            SizeScaler.Scale(stage);
        }

        private void OnWinning()
        {
            Game.Win();
        }

        private void OnEnemyContacted()
        {
            Game.Lose();
        }

        private void OnScoreGained(float value)
        {
            Animation.PlayAttack();
            Model.IncreaseScore(value);
        }

        private void OnBoosterEntered(IBooster booster)
        {
            if(BoosterService.TryInsert(booster) == true)
                SoundReproducer.PlayClip(SoundType.Boost);
        }

        private void OnHid()
        {
            Mover.ProhibitMove();
            Animation.PlayHide();
            EffectReproducer.PlayEffect(EffectType.Hide);
            SoundReproducer.PlayClip(SoundType.Hide);
        }

        private void OnShowed()
        {
            Mover.AllowMove();
            Animation.PlayIdle();
        }

        private void OnSpitCasted()
        {
            SoundReproducer.PlayClip(SoundType.Spit);
            Animation.PlayAttack();
        }
    }
}