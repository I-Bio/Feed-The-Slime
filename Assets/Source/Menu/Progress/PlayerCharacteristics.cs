using System;
using UnityEngine;

namespace Menu
{
    [Serializable]
    public class PlayerCharacteristics : IReadOnlyCharacteristics
    {
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float ScorePerEat { get; set; }
        [field: SerializeField] public int LifeCount { get; set; }
        [field: SerializeField] public int CrystalsCount { get; set; }
        [field: SerializeField] public int CompletedLevels { get; set; }
        [field: SerializeField] public bool SpitObtained { get; set; }
    }
}