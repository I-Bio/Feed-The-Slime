using Spawners;

namespace Foods
{
    public class DissolvedEdible : EdiblePart
    {
        private SpawnableObject _spawnable;

        public override void Initialize(float scorePerEat)
        {
            _spawnable = GetComponent<SpawnableObject>();
            base.Initialize(scorePerEat);
        }
        
        protected override void OnEat()
        {
            _spawnable.Push();
        }
    }
}