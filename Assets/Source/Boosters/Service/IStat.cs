using UnityEngine;

namespace Boosters
{
    public interface IStat
    {
        public float LifeTime { get; }

        public Sprite Icon { get; }

        public void Accept(IBoosterVisitor visitor);
    }
}