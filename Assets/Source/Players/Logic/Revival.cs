using System;
using TMPro;
using UnityEngine;

namespace Players
{
    public class Revival : MonoBehaviour, IRevival
    {
        private Transform _player;
        private Vector3 _startPosition;
        private int _maxLifeCount;
        private int _currentLifeCount;
        private GameObject _holder;
        private TextMeshProUGUI _holderText;

        public event Action Revived;

        public void Initialize(Transform player, int maxLifeCount, GameObject holder, TextMeshProUGUI holderText)
        {
            _player = player;
            _startPosition = _player.position;
            _maxLifeCount = maxLifeCount;
            _holder = holder;
            _holderText = holderText;

            if (_maxLifeCount <= (int)ValueConstants.One)
                return;

            _holder.SetActive(true);
            _holderText.SetText((_maxLifeCount - _currentLifeCount).ToString());
        }

        public bool TryRevive()
        {
            if (_currentLifeCount >= _maxLifeCount)
                return false;

            _holderText.SetText((_maxLifeCount - _currentLifeCount).ToString());
            _currentLifeCount++;

            if (_holder.activeSelf == true && _currentLifeCount >= _maxLifeCount)
                _holder.SetActive(false);

            Revive();
            return true;
        }

        public void Revive()
        {
            _player.position = _startPosition;
            Revived?.Invoke();
        }
    }
}