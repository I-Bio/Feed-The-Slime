using UnityEngine;

namespace Players
{
    public class EffectReproducer : MonoBehaviour
    {
        private ParticleSystem[] _effects;

        public void Initialize(ParticleSystem[] effects)
        {
            _effects = effects;
        }
        
        public void Play(EffectType effect)
        {
            _effects[(int)effect].Play();
        }

        public void PlayAt(EffectType effect, Vector3 position)
        {
            _effects[(int)effect].transform.position = position;
            _effects[(int)effect].Play();
        }
    }
}