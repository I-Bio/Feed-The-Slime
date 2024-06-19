namespace Boosters
{
    public class Combined<T> : IInsertable<T>
        where T : class
    {
        private T _previous;

        public T Next { get; private set; }

        public void Insert(T stat)
        {
            _previous = stat;

            if (Next != null)
            {
                IInsertable<T> insertable = stat as IInsertable<T>;
                insertable?.Insert(Next);
            }

            Next = _previous;
        }

        public void Clear()
        {
            Next = null;
            _previous = null;
        }
    }
}