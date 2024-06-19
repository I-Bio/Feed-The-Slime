using System.Collections.Generic;

namespace Boosters
{
    public class StatCombiner<T>
        where T : class
    {
        private readonly LinkedList<T> Stats = new ();
        private readonly Combined<T> Combined;

        public StatCombiner(Combined<T> combined)
        {
            Combined = combined;
        }

        public void Add(T stat)
        {
            Stats.AddLast(stat);
        }

        public void AddAfterFirst(T stat)
        {
            Stats.AddAfter(Stats.First, stat);
        }

        public void ChangeBoost(T stat)
        {
            if (Stats.Find(stat) != null)
            {
                Stats.Remove(stat);
                return;
            }

            Add(stat);
        }

        public T GetRecombined()
        {
            Combined.Clear();

            foreach (T stat in Stats)
                Combined.Insert(stat);

            return Combined as T;
        }
    }
}