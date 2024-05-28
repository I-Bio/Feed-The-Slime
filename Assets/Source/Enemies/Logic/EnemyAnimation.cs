using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        private Animator _animator;
        private Action OnAnimationPlayedCallback;

        public void Initialize(Animator animator, Action onInitializeCallback)
        {
            _animator = animator;
            onInitializeCallback?.Invoke();
            _animator.ResetTrigger(EnemyAnimations.Idle.ToString());
        }

        public void Play(EnemyAnimations animation, Action onAnimationPlayedCallback = null)
        {
            OnAnimationPlayedCallback = onAnimationPlayedCallback;
            _animator.SetTrigger(animation.ToString());
        }

        public void InvokeCallback()
        {
            OnAnimationPlayedCallback?.Invoke();
            OnAnimationPlayedCallback = null;
        }
    }
}