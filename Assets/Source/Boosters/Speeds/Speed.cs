using UnityEngine;

namespace Boosters
{
    public class Speed : Movable
    {
        public Speed(float value, float lifeTime = 0f, Sprite icon = null, string sign = "") : base(value, lifeTime, icon, sign) {}
        
        public override float GetSpeed()
        {
            return Value;
        }
    }
}