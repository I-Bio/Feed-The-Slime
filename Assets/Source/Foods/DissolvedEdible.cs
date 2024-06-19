using Spawners;

namespace Foods
{
    public class DissolvedEdible : EdiblePart
    {
        private SpawnableObject _spawnable;

        public override void OnInitialize()
        {
            _spawnable = GetComponent<SpawnableObject>();
        }

        public override void OnEatingCompletion()
        {
            _spawnable.Push();
        }
    }
}