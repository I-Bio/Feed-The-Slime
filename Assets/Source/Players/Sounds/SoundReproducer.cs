using UnityEngine;

namespace Players
{
    public class SoundReproducer : MonoBehaviour
    {
        [SerializeField] private AudioSource[] _sources;

        public void PlayClip(SoundType sound)
        {
            _sources[(int)sound].Play();
        }
    }
}