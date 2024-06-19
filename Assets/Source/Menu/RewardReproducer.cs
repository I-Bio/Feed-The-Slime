using System;
using DG.Tweening;
using Spawners;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Menu
{
    public class RewardReproducer : ObjectPool
    {
        [SerializeField] private AudioSource _sound;
        [SerializeField] private float _duration;
        [SerializeField] private float _durationOffSet;
        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _creator;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private float _offsetX;
        [SerializeField] private float _offsetY;

        private RewardGem[] _gems;
        private Vector2 _size;
        private int _pointer;
        private bool _isPlaying;
        private Action _callBack;

        public void Initialize(RewardGem template)
        {
            base.Initialize(template);
            _size = template.RectTransform.sizeDelta;
        }

        public void Reproduce(int count, Action callBack = null)
        {
            if (_isPlaying == true)
                return;

            _isPlaying = true;
            _gems = new RewardGem[count];
            _callBack = callBack;
            Vector3 targetPosition = _parent.TransformPoint(_target.localPosition);

            for (int i = 0; i < _gems.Length; i++)
            {
                Vector2 position = new Vector2(Random.Range(-_offsetX, _offsetX), Random.Range(-_offsetY, _offsetY));
                float duration = Random.Range(_duration - _durationOffSet, _duration + _durationOffSet);
                _gems[i] = Pull<RewardGem>(_creator);
                _gems[i].RectTransform.anchoredPosition = position;
                _gems[i].RectTransform.DOMove(targetPosition, duration).SetUpdate(true);
                _gems[i].RectTransform.DOSizeDelta(_target.sizeDelta, duration).SetUpdate(true).OnComplete(PlaySound);
            }
        }

        private void PlaySound()
        {
            _sound.Play();
            _gems[_pointer].Push();
            _gems[_pointer].RectTransform.position = Vector3.zero;
            _gems[_pointer].RectTransform.sizeDelta = _size;
            _pointer++;

            if (_pointer == _gems.Length)
                Stop();
        }

        private void Stop()
        {
            _isPlaying = false;
            _pointer = 0;
            _callBack?.Invoke();
        }
    }
}