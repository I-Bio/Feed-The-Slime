using UnityEngine;

namespace Boosters
{
    public class MultipleScore : CalculableScore, IInsertable<ICalculableScore>
    {
        private ICalculableScore _calculable;

        public MultipleScore(
            ICalculableScore calculable,
            float scaler,
            float lifeTime = 0f,
            Sprite icon = null,
            string sign = "")
            : base(scaler, lifeTime, icon, sign)
        {
            _calculable = calculable;
        }

        public void Insert(ICalculableScore stat)
        {
            _calculable = stat;
        }

        public override float CalculateScore(float score)
        {
            return _calculable.CalculateScore(score) * Value;
        }
    }
}