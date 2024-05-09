namespace Boosters
{
    public class Combined<T> : IInsertable<T> where T : class
    {
        private T Next;
        private T Previous;
        
        protected T Stat => Next;
        
        public void Insert(T stat)
        {
            Previous = stat;
            
            if (Next != null)
            {
                IInsertable<T> insertable = stat as IInsertable<T>;
                insertable?.Insert(Next);
            }
            
            Next = Previous;
        }
        
        public void Clear()
        {
            Next = null;
            Previous = null;
        }
    }
}