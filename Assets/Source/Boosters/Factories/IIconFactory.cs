using System.Collections.Generic;
using Spawners;

namespace Boosters
{
    public interface IIconFactory
    {
        public KeyValuePair<SpawnableObject, IStat> Create(IStat stat);
    }
}