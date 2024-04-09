using System;
using DG.Tweening;
using UnityEngine;

namespace Menu
{
    public class RewardReproducer : MonoBehaviour
    {
        [SerializeField] private AudioSource _sound;
        [SerializeField] private GameObject _holder;
        [SerializeField] private float _durationPerPoint;
        [SerializeField] private RectTransform[] _gems;
        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _parent;
        
        private int _pointer;
        private Action _callback;

        public void Reproduce(Action callback)
        {
            Vector2 position = _parent.TransformPoint(_target.localPosition);
            Sequence sequence = DOTween.Sequence();
            _callback = callback;
            _holder.SetActive(true);

            foreach (RectTransform gem in _gems)
            {
                gem.gameObject.SetActive(true);
                
                sequence
                    .Append(gem.DOMove(position, _durationPerPoint).OnComplete(PlaySound))
                    .Join(gem.DOSizeDelta(_target.sizeDelta, _durationPerPoint));
            }

            sequence.Play().OnComplete(Stop);
        }

        private void PlaySound()
        {
            _sound.Play();
            _gems[_pointer].gameObject.SetActive(false);
            _pointer++;
        }

        private void Stop()
        {
            _holder.SetActive(false);
            _callback?.Invoke();
        }
    }
}