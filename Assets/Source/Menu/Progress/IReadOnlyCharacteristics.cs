namespace Menu
{
    public interface IReadOnlyCharacteristics
    {
        public float Speed { get; }
        public float ScorePerEat{ get; }
        public int LifeCount{ get; }
        public int CrystalsCount{ get; }
        public int CompletedLevels{ get; }
        public int AdvertAccumulation { get; }
        public bool DidObtainSpit{ get; }
        public bool IsAllowedShowInter { get; }
    }
}