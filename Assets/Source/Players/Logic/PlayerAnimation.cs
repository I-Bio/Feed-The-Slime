using UnityEngine;

namespace Players
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;
        
        private string _eat;
        private string _idle;
        private string _hide;

        public void Initialize(Animator animator, string eat, string idle, string hide)
        {
            _animator = animator;
            _eat = eat;
            _idle = idle;
            _hide = hide;
        }

        public void PlayAttack()
        {
            _animator.SetTrigger(_eat);
        }

        public void PlayIdle()
        {
            _animator.SetTrigger(_idle);
        }

        public void PlayHide()
        {
            _animator.SetTrigger(_hide);
        }
    }
}