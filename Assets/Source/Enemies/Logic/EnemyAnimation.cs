using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        private Animator _animator;
        private string _move;
        private string _idle;
        private bool _isMoved;

        public void Initialize(Animator animator, string idle, string move)
        {
            _animator = animator;
            _idle = idle;
            _move = move;
        }

        public void PlayMove()
        {
            if (_isMoved == true)
                return;

            _isMoved = true;
            _animator.SetTrigger(_move);
        }

        public void PlayIdle()
        {
            _isMoved = false;
            _animator.SetTrigger(_idle);
        }
    }
}