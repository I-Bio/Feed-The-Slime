using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        private Animator _animator;
        private Action _onPlayedCallback;

        public void Initialize(Animator animator, Action onInitializeCallback)
        {
            _animator = animator;
            onInitializeCallback?.Invoke();
            _animator.ResetTrigger(EnemyAnimations.Idle.ToString());
        }

        public void Play(EnemyAnimations animation, Action onPlayedCallback)
        {
            _onPlayedCallback = onPlayedCallback;
            _animator.SetTrigger(animation.ToString());
        }

        public void InvokeCallback()
        {
            _onPlayedCallback?.Invoke();
            _onPlayedCallback = null;
        }
    }
}