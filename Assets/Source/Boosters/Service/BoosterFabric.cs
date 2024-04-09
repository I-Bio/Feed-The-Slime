using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boosters
{
    public class BoosterFabric : IBoosterFabricVisitor
    {
        private const string Plus = "+";
        private const string Multiplication = "*";

        private readonly IMovable _playerSpeed;
        private readonly ICalculableScore _playerScore;

        private readonly Sprite _speedIcon;
        private readonly Sprite _scoreIcon;

        private readonly float[] _scaleValues;
        private readonly float[] _additionalValues;

        public BoosterFabric(IMovable playerSpeed, ICalculableScore playerScore, float[] scaleValues,
            float[] additionalValues, Sprite speedIcon, Sprite scoreIcon)
        {
            _playerSpeed = playerSpeed;
            _playerScore = playerScore;
            _scaleValues = scaleValues;
            _additionalValues = additionalValues;
            _speedIcon = speedIcon;
            _scoreIcon = scoreIcon;
        }

        public KeyValuePair<IStatBuffer, string> CreateBoost(BoosterType type, float lifeTime)
        {
            return type switch
            {
                BoosterType.SpeedAdder => Visit(null as AdditionalSpeed, lifeTime),
                BoosterType.SpeedScaler => Visit(null as MultipleSpeed, lifeTime),
                BoosterType.ScoreAdder => Visit(null as AdditionalScore, lifeTime),
                BoosterType.ScoreScaler => Visit(null as MultipleScore, lifeTime),
                _ => throw new NullReferenceException(nameof(type))
            };
        }

        public KeyValuePair<IStatBuffer, string> Visit(AdditionalSpeed type, float lifeTime)
        {
            float value = _additionalValues.GetRandom();
            return new KeyValuePair<IStatBuffer, string>(
                new AdditionalSpeed(_playerSpeed, value, lifeTime, _speedIcon),
                $"{Plus}{value}");
        }

        public KeyValuePair<IStatBuffer, string> Visit(MultipleSpeed type, float lifeTime)
        {
            float value = _scaleValues.GetRandom();
            return new KeyValuePair<IStatBuffer, string>(
                new MultipleSpeed(_playerSpeed, value, lifeTime, _speedIcon),
                $"{Multiplication}{value}");
        }

        public KeyValuePair<IStatBuffer, string> Visit(AdditionalScore type, float lifeTime)
        {
            float value = _additionalValues.GetRandom();
            return new KeyValuePair<IStatBuffer, string>(
                new AdditionalScore(_playerScore, value, lifeTime, _scoreIcon),
                $"{Plus}{value}");
        }

        public KeyValuePair<IStatBuffer, string> Visit(MultipleScore type, float lifeTime)
        {
            float value = _scaleValues.GetRandom();
            return new KeyValuePair<IStatBuffer, string>(
                new MultipleScore(_playerScore, value, lifeTime, _scoreIcon),
                $"{Multiplication}{value}");
        }
    }
}