using System;
using Random = UnityEngine.Random;

namespace Boosters
{
    public class BoosterFabric
    {
        private readonly IMovable _playerSpeed;
        private readonly ICalculableScore _playerScore;
        private readonly float[] _scaleValues;
        private readonly float[] _additionalValues;

        public BoosterFabric(IMovable playerSpeed, ICalculableScore playerScore, float[] scaleValues, float[] additionalValues)
        {
            _playerSpeed = playerSpeed;
            _playerScore = playerScore;
            _scaleValues = scaleValues;
            _additionalValues = additionalValues;
        }
        
        public IStatBuffer CreateBoost(BoosterType type, float lifeTime)
        {
            return type switch
            {
                BoosterType.SpeedAdder => new AdditionalSpeed(_playerSpeed, _additionalValues.GetRandom(), lifeTime),
                BoosterType.SpeedScaler => new MultipleSpeed(_playerSpeed, _scaleValues.GetRandom(), lifeTime),
                BoosterType.ScoreAdder => new AdditionalScore(_playerScore, _additionalValues.GetRandom(), lifeTime),
                BoosterType.ScoreScaler => new MultipleScore(_playerScore, _scaleValues.GetRandom(), lifeTime),
                _ => throw new NullReferenceException(nameof(type))
            };
        }
    }
}