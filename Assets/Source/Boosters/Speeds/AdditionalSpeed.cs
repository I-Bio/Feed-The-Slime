using UnityEngine;

namespace Boosters
{
    public class AdditionalSpeed : Movable, IInsertable<IMovable>
    {
        private IMovable _movable;

        public AdditionalSpeed(IMovable movable, float additionValue, float lifeTime = 0f, Sprite icon = null, string sign = "") : base(additionValue, lifeTime, icon, sign)
        {
            _movable = movable;
        }
        
        public void Insert(IMovable stat)
        {
            _movable = stat;
        }

        public override float GetSpeed()
        {
            return _movable.GetSpeed() + Value;
        }
    }
}