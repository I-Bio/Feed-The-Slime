using System;
using Boosters;
using Menu;
using Spawners;
using UnityEngine;

namespace Players
{
    public class PlayerPresenter : IPresenter
    {
        private readonly Player Model;
        private readonly PlayerCollisionDetector CollisionDetector;
        private readonly PlayerScanner Scanner;
        private readonly SizeScaler SizeScaler;
        private readonly LevelBar LevelBar;
        private readonly StageBar StageBar;
        private readonly PlayerAnimation Animation;
        private readonly AbilityCaster Caster;
        private readonly EffectReproducer EffectReproducer;
        private readonly SoundReproducer SoundReproducer;
        private readonly EatableSpawner Spawner;
        private readonly IUsable BoosterService;
        private readonly IMover Mover;
        private readonly IGame Game;
        private readonly IRevival Revival;
        private readonly Action<float> ProgressChangedCallback;

        public PlayerPresenter(Player model, PlayerCollisionDetector collisionDetector, PlayerScanner scanner,
            SizeScaler sizeScaler, LevelBar levelBar, StageBar stageBar, IUsable boosterService,
            PlayerAnimation animation, AbilityCaster caster, IMover mover, EffectReproducer effectReproducer,
            SoundReproducer soundReproducer, EatableSpawner spawner, IGame game, IRevival revival, Action<float> progressChangedCallback)
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
            Spawner = spawner;
            Game = game;
            Revival = revival;
            ProgressChangedCallback = progressChangedCallback;
        }

        public void Enable()
        {
            Model.Loaded += OnLoaded;
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
            Spawner.Spawned += OnSpawned;
            Revival.Revived += OnRevived;
            
            Model.Load();
        }

        public void Disable()
        {
            Model.Loaded -= OnLoaded;
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
            Spawner.Spawned -= OnSpawned;
            Revival.Revived -= OnRevived;
        }

        private void OnLoaded(SatietyStage stage, int level, float score, int maxScore)
        {
            LevelBar.ChangeScore(score, maxScore);
            LevelBar.SetLevel(level);

            for (int i = 0; i < (int)stage; i++)
                StageBar.Increase();
            
            StageBar.ChangeValue(score);
            Caster.SetStage(stage);
            Scanner.SetStage(stage);
            CollisionDetector.SetStage(stage);
            Game.SetStage(stage);
            SizeScaler.Scale(stage);
            Mover.Scale(stage);
        }
        
        private void OnScoreChanged(float score, int maxScore)
        {
            SoundReproducer.PlayClip(SoundType.ScoreGain);
            LevelBar.ChangeScore(score, maxScore);
            StageBar.ChangeValue(score);
            ProgressChangedCallback?.Invoke(score);
        }

        private void OnLevelIncreased(int level)
        {
            SoundReproducer.PlayClip(SoundType.LevelUp);
            LevelBar.SetLevel(level);
        }

        private void OnSizeIncreased(SatietyStage stage)
        {
            SizeScaler.Scale(stage, OnScaled);
            Caster.SetStage(stage);
            Scanner.SetStage(stage);
            CollisionDetector.SetStage(stage);
            Game.SetStage(stage);
            StageBar.Increase();
            Mover.Scale(stage);
        }

        private void OnScaled(Vector3 position)
        {
            EffectReproducer.PlayAt(EffectType.StageUp, position);
            SoundReproducer.PlayClip(SoundType.StageUp);
        }

        private void OnWinning()
        {
            ProgressChangedCallback?.Invoke((float)ValueConstants.Zero);
            Game.Win();
        }

        private void OnEnemyContacted()
        {
            ProgressChangedCallback?.Invoke((float)ValueConstants.Zero);
            Game.Lose();
        }

        private void OnScoreGained(float value)
        {
            Animation.PlayAttack();
            EffectReproducer.Play(EffectType.WaterDrop);
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
            EffectReproducer.Play(EffectType.Hide);
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

        private void OnSpawned(Contactable contactable, ISelectable selectable)
        {
            CollisionDetector.AddContactable(contactable);
            Scanner.AddSelectable(selectable);
        }

        private void OnRevived()
        {
            EffectReproducer.Play(EffectType.Heal);
            SoundReproducer.PlayClip(SoundType.StageUp);
        }
    }
}