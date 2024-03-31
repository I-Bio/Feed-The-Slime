using Players.Sounds;
using UnityEngine;

namespace Players
{
    public class SoundReproducer : MonoBehaviour
    {
        [SerializeField] private AudioSource[] _clips;

        public void PlayClip(SoundType sound)
        {
            _clips[(int)sound].Play();
        }
    }
}