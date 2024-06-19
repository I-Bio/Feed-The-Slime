using UnityEngine;

namespace Boosters
{
    public class AdditionalScore : CalculableScore, IInsertable<ICalculableScore>
    {
        private ICalculableScore _calculable;

        public AdditionalScore(
            ICalculableScore calculable,
            float additionValue,
            float lifeTime = 0f,
            Sprite icon = null,
            string sign = "")
            : base(additionValue, lifeTime, icon, sign)
        {
            _calculable = calculable;
        }

        public void Insert(ICalculableScore stat)
        {
            _calculable = stat;
        }

        public override float CalculateScore(float score)
        {
            return _calculable.CalculateScore(score) + Value;
        }
    }
}