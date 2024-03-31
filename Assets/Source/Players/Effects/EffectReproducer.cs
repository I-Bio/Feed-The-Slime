using UnityEngine;

namespace Players
{
    public class EffectReproducer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _effects;

        public void PlayEffect(EffectType effect)
        {
            _effects[(int)effect].Play();
        }
    }
}