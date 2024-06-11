using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boosters
{
    public class BoosterFactory : IFactory<Booster>
    {
        private const string Plus = "+";
        private const string Multiplication = "*";
        
        private readonly float[] ScaleValues;
        private readonly float[] AdditionalValues;
        private readonly BoosterType[] BoosterTypes;
        private readonly List<Transform> SpawnPoints;

        private readonly Booster Template;
        private readonly Sprite SpeedIcon;
        private readonly Sprite ScoreIcon;
        private readonly float MaxLifeTime;
        private readonly float MinLifeTime;
        private readonly Vector3 Offset;
        private readonly Func<Vector3, Booster> Pulling;

        public BoosterFactory(float[] scaleValues, float[] additionalValues,
            Sprite speedIcon, Sprite scoreIcon, float maxLifeTime, float minLifeTime,
            Transform pointsHolder, Vector3 offset, Func<Vector3, Booster> pullingBooster)
        {
            ScaleValues = scaleValues;
            AdditionalValues = additionalValues;
            SpeedIcon = speedIcon;
            ScoreIcon = scoreIcon;
            MaxLifeTime = maxLifeTime;
            MinLifeTime = minLifeTime;
            Offset = offset;
            Pulling = pullingBooster;
            SpawnPoints = new List<Transform>();
            CollectPoints(pointsHolder);
            BoosterTypes = (BoosterType[])Enum.GetValues(typeof(BoosterType));
        }

        public Booster Create()
        {
            Vector3 position = SpawnPoints.GetRandom().position + Offset;
            return Pulling?.Invoke(position).Initialize(CreateStat());
        }

        private IStat CreateStat()
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
        
        private void CollectPoints(Transform pointsHolder)
        {
            for (int i = 0; i < pointsHolder.childCount; i++)
                SpawnPoints.Add(pointsHolder.GetChild(i));
        }
    }
}