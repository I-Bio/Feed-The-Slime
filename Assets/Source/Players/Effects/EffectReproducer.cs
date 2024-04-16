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
        
        public void PlayEffect(EffectType effect)
        {
            _effects[(int)effect].Play();
        }
    }
}