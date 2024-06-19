using System;
using System.Collections.Generic;
using Enemies.Hide;
using Players;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyStatesFactory : IFactory<Dictionary<EnemyStates, FiniteStateMachineState>>
    {
        private readonly EnemyTypes Type;
        private readonly FiniteStateMachine Machine;
        private readonly IHidden Player;
        private readonly Transform Transform;
        private readonly EnemyAnimation Animation;
        private readonly SatietyStage Stage;
        private readonly float FollowDistance;
        private readonly Vector3 StartPosition;
        private readonly float IdleOffset;
        private readonly NavMeshAgent Agent;
        private readonly AudioSource Sound;
        private readonly ParticleSystem Particle;
        private readonly Swarm Swarm;
        private readonly IPlayerVisitor Visitor;

        public EnemyStatesFactory(
            EnemyTypes type,
            FiniteStateMachine machine,
            IHidden player,
            Transform transform,
            EnemyAnimation animation,
            SatietyStage stage,
            float followDistance,
            Vector3 startPosition,
            float idleOffset,
            NavMeshAgent agent,
            AudioSource sound,
            IPlayerVisitor visitor,
            ParticleSystem particle,
            Swarm swarm)
        {
            Type = type;
            Machine = machine;
            Player = player;
            Transform = transform;
            Animation = animation;
            Stage = stage;
            FollowDistance = followDistance;
            StartPosition = startPosition;
            IdleOffset = idleOffset;
            Agent = agent;
            Sound = sound;
            Particle = particle;
            Swarm = swarm;
            Visitor = visitor;
        }

        public Dictionary<EnemyStates, FiniteStateMachineState> Create()
        {
            return Type switch
            {
                EnemyTypes.Mover => CreateMover(),
                EnemyTypes.Toxin => CreateToxin(),
                EnemyTypes.Swarm => CreateSwarm(),
                EnemyTypes.Hider => CreateHider(),
                _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null),
            };
        }

        private Dictionary<EnemyStates, FiniteStateMachineState> CreateMover()
        {
            return new Dictionary<EnemyStates, FiniteStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyIdleState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyEscapeState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Agent)
                },
                {
                    EnemyStates.Interact,
                    new EnemyFollowState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Agent)
                },
            };
        }

        private Dictionary<EnemyStates, FiniteStateMachineState> CreateToxin()
        {
            return new Dictionary<EnemyStates, FiniteStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyIdleState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyStaticAvoidState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Interact,
                    new EnemyToxinState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Sound,
                        Visitor,
                        Particle)
                },
            };
        }

        private Dictionary<EnemyStates, FiniteStateMachineState> CreateSwarm()
        {
            Swarm.Initialize();
            return new Dictionary<EnemyStates, FiniteStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyIdleState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyStaticAvoidState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Interact,
                    new EnemySwarmState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Sound,
                        Visitor,
                        Swarm)
                },
            };
        }

        private Dictionary<EnemyStates, FiniteStateMachineState> CreateHider()
        {
            return new Dictionary<EnemyStates, FiniteStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyHideState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Particle)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyEscapeState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Agent)
                },
                {
                    EnemyStates.Action,
                    new EnemyShowState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Particle)
                },
                {
                    EnemyStates.Interact,
                    new EnemyFollowState(
                        Machine,
                        Player,
                        Transform,
                        Animation,
                        Stage,
                        FollowDistance,
                        StartPosition,
                        IdleOffset,
                        Agent)
                },
            };
        }
    }
}