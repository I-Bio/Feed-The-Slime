using UnityEngine;

namespace Boosters
{
    public class MultipleSpeed : Movable, IInsertable<IMovable>
    {
        private IMovable _movable;

        public MultipleSpeed(IMovable movable, float scaler, float lifeTime = 0f,
            Sprite icon = null, string sign = "") : base(scaler, lifeTime, icon, sign)
        {
            _movable = movable;
        }
        
        public void Insert(IMovable stat)
        {
            _movable = stat;
        }

        public override float GetSpeed()
        {
            return _movable.GetSpeed() * Value;
        }
    }
}