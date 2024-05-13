using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyMover))]
    public class Swarm : MonoBehaviour
    {
        private EnemyMover _mover;
        private AudioSource _source;
        
        public void Initialize(float delay, AudioSource source)
        {
            _mover = GetComponent<EnemyMover>();
            _source = source;
            _mover.Initialize(delay);
        }

        public void Move(Vector3 position)
        {
            _mover.InteractInClose(position);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _source.Play();
        }

        public void Hide()
        {
            _source.Stop();
            gameObject.SetActive(false);
        }
    }
}