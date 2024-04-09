using UnityEngine;

namespace Boosters
{
    public interface IStatBuffer
    {
        public float LifeTime { get;}
        public Sprite Icon { get;}
        
        public void Accept(IBoosterVisitor visitor);
    }
}