using Foods;
using UnityEngine;

namespace Spawners
{
    public interface IEatableFactory
    {
        public void Create(Vector3 contactPosition, EdiblePart food);
    }
}