using System;
using System.Collections.Generic;
using Enemies.Hide;
using Players;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyStatesFactory : IFactory<Dictionary<EnemyStates, FinalStateMachineState>>
    {
        private readonly EnemyTypes Type;
        private readonly FinalStateMachine Machine;
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
        private readonly float ThinkDelay;

        public EnemyStatesFactory(EnemyTypes type, FinalStateMachine machine, IHidden player, Transform transform,
            EnemyAnimation animation, SatietyStage stage, float followDistance, Vector3 startPosition, float idleOffset,
            NavMeshAgent agent, AudioSource sound, IPlayerVisitor visitor, ParticleSystem particle, Swarm swarm, float thinkDelay)
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
            ThinkDelay = thinkDelay;
        }

        public Dictionary<EnemyStates, FinalStateMachineState> Create()
        {
            return Type switch
            {
                EnemyTypes.Mover => CreateMover(),
                EnemyTypes.Toxin => CreateToxin(),
                EnemyTypes.Swarm => CreateSwarm(),
                EnemyTypes.Hider => CreateHider(),
                _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null)
            };
        }

        private Dictionary<EnemyStates, FinalStateMachineState> CreateMover()
        {
            return new Dictionary<EnemyStates, FinalStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyIdleState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyEscapeState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Agent)
                },
                {
                    EnemyStates.Interact,
                    new EnemyFollowState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Agent)
                }
            };
        }

        private Dictionary<EnemyStates, FinalStateMachineState> CreateToxin()
        {
            return new Dictionary<EnemyStates, FinalStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyIdleState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyStaticAvoidState(Machine, Player, Transform, Animation, Stage, FollowDistance,
                        StartPosition, IdleOffset)
                },
                {
                    EnemyStates.Interact,
                    new EnemyToxinState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Sound, Visitor, Particle)
                }
            };
        }

        private Dictionary<EnemyStates, FinalStateMachineState> CreateSwarm()
        {
            Swarm.Initialize(ThinkDelay);
            return new Dictionary<EnemyStates, FinalStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyIdleState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyStaticAvoidState(Machine, Player, Transform, Animation, Stage, FollowDistance,
                        StartPosition, IdleOffset)
                },
                {
                    EnemyStates.Interact,
                    new EnemySwarmState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Sound, Visitor, Swarm)
                }
            };
        }
        
        private Dictionary<EnemyStates, FinalStateMachineState> CreateHider()
        {
            return new Dictionary<EnemyStates, FinalStateMachineState>
            {
                {
                    EnemyStates.Idle,
                    new EnemyHideState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Particle)
                },
                {
                    EnemyStates.Avoid,
                    new EnemyEscapeState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Agent)
                },
                {
                    EnemyStates.Action,
                    new EnemyShowState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Particle)
                },
                {
                    EnemyStates.Interact,
                    new EnemyFollowState(Machine, Player, Transform, Animation, Stage, FollowDistance, StartPosition,
                        IdleOffset, Agent)
                }
            };
        }
    }
}