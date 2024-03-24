namespace Boosters
{
    public interface IStatBuffer
    {
        public float LifeTime { get; set; }
        
        public void Accept(IBoosterVisitor visitor);
    }
}