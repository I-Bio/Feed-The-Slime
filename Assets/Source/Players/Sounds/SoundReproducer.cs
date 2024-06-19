using UnityEngine;

namespace Players
{
    public class SoundReproducer : MonoBehaviour
    {
        private AudioSource[] _sources;

        public void Initialize(AudioSource[] sources)
        {
            _sources = sources;
        }

        public void PlayClip(SoundType sound)
        {
            _sources[(int)sound].Play();
        }
    }
}