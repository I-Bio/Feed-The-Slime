using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boosters
{
    public class BoosterStatFactory : IFactory<IStat>
    {
        private const string Plus = "+";
        private const string Multiplication = "*";
        
        private readonly float[] ScaleValues;
        private readonly float[] AdditionalValues;
        private readonly BoosterType[] BoosterTypes;

        private readonly Sprite SpeedIcon;
        private readonly Sprite ScoreIcon;
        private readonly float MaxLifeTime;
        private readonly float MinLifeTime;

        public BoosterStatFactory(float[] scaleValues, float[] additionalValues,
            Sprite speedIcon, Sprite scoreIcon, float maxLifeTime, float minLifeTime)
        {
            ScaleValues = scaleValues;
            AdditionalValues = additionalValues;
            SpeedIcon = speedIcon;
            ScoreIcon = scoreIcon;
            MaxLifeTime = maxLifeTime;
            MinLifeTime = minLifeTime;
            BoosterTypes = (BoosterType[])Enum.GetValues(typeof(BoosterType));
        }

        public IStat Create()
        {
            return BoosterTypes.GetRandom() switch
            {
                BoosterType.SpeedAdder =>
                    new AdditionalSpeed(new Speed((float)ValueConstants.One, (float)ValueConstants.Zero, SpeedIcon),
                        AdditionalValues.GetRandom(),
                        Random.Range(MinLifeTime, MaxLifeTime), SpeedIcon, Plus),

                BoosterType.SpeedScaler =>
                    new MultipleSpeed(new Speed((float)ValueConstants.One, (float)ValueConstants.Zero, SpeedIcon),
                        ScaleValues.GetRandom(),
                        Random.Range(MinLifeTime, MaxLifeTime), SpeedIcon, Multiplication),

                BoosterType.ScoreAdder =>
                    new AdditionalScore(new Score((float)ValueConstants.One), AdditionalValues.GetRandom(),
                        Random.Range(MinLifeTime, MaxLifeTime), ScoreIcon, Plus),

                BoosterType.ScoreScaler =>
                    new MultipleScore(new Score((float)ValueConstants.One), ScaleValues.GetRandom(),
                        Random.Range(MinLifeTime, MaxLifeTime), ScoreIcon, Multiplication),

                _ => throw new NullReferenceException(nameof(BoosterType))
            };
        }
    }
}