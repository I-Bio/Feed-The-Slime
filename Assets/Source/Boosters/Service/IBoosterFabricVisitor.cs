using System.Collections.Generic;
using UnityEngine;

namespace Boosters
{
    public interface IBoosterFabricVisitor
    {
        public KeyValuePair<IStatBuffer, KeyValuePair<string, Sprite>> Visit(AdditionalSpeed type, float lifeTime);
        public KeyValuePair<IStatBuffer, KeyValuePair<string, Sprite>> Visit(MultipleSpeed type, float lifeTime);
        public KeyValuePair<IStatBuffer, KeyValuePair<string, Sprite>> Visit(AdditionalScore type, float lifeTime);
        public KeyValuePair<IStatBuffer, KeyValuePair<string, Sprite>> Visit(MultipleScore type, float lifeTime);
    }
}