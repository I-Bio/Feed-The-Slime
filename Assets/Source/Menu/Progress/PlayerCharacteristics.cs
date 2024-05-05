using System;
using UnityEngine;

namespace Menu
{
    [Serializable]
    public class PlayerCharacteristics : IReadOnlyCharacteristics
    {
        [field: SerializeField] public float Speed { get; set; }
        public float ScorePerEat { get; set; }
        public int LifeCount { get; set; }
        public int CrystalsCount { get; set; }
        public int CompletedLevels { get; set; }
        public int AdvertAccumulation { get; set; }
        public bool DidObtainSpit { get; set; }
        public bool IsAllowedShowInter { get; set; }
        public bool DidPassGuide { get; set; }
        public float GameVolume { get; set; }
        public float MusicVolume { get; set; }
    }
}