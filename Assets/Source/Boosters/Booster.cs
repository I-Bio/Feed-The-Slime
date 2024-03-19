using Spawners;

namespace Boosters
{
    public class Booster : SpawnableObject, IBooster
    {
        private IStatBuffer _boost;

        public void Initialize(IStatBuffer boost)
        {
            _boost = boost;
        }

        public void Use()
        {
            Push();
        }
        
        public IStatBuffer GetBoost()
        {
            return _boost; 
        }
    }
}