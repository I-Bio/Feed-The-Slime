using System;

namespace Players
{
    public interface IRevival
    {
        public event Action Revived;
    }
}