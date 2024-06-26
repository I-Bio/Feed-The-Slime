﻿using UnityEngine;

namespace Enemies
{
    public class EnemyStaticAvoidState : EnemyAvoidState
    {
        public EnemyStaticAvoidState(
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset)
            : base(machine, player, transform, animation, stage, followDistance, startPosition, idleOffset)
        {
        }

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Idle);
        }

        public override void Update()
        {
        }
    }
}