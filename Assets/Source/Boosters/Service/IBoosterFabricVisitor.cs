using System.Collections.Generic;
using UnityEngine;

namespace Boosters
{
    public interface IBoosterFabricVisitor
    {
        public KeyValuePair<IStatBuffer, string> Visit(AdditionalSpeed type, float lifeTime);
        public KeyValuePair<IStatBuffer, string> Visit(MultipleSpeed type, float lifeTime);
        public KeyValuePair<IStatBuffer, string> Visit(AdditionalScore type, float lifeTime);
        public KeyValuePair<IStatBuffer, string> Visit(MultipleScore type, float lifeTime);
    }
}