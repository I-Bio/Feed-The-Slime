using System;
using UnityEngine;

namespace Menu
{
    public class TransferService : MonoBehaviour, ITransferService
    {
        public static TransferService Instance { get; private set; }

        private IReadOnlyCharacteristics _characteristics;
        private int _rewardCount;
        private bool _canReceive;
        private bool _didLevelPassed;

        public IReadOnlyCharacteristics Characteristics => _characteristics;
        public int Reward => _rewardCount;
        public bool DidLevelPassed => _didLevelPassed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetStats(int value, IReadOnlyCharacteristics characteristics)
        {
            _rewardCount = value;
            _characteristics = characteristics;
        }

        public void AllowReceive()
        {
            _canReceive = true;
        }

        public void PassLevel()
        {
            _didLevelPassed = true;
        }

        public void MultiplyIt(float value)
        {
            _rewardCount = Mathf.CeilToInt(_rewardCount * value);
        }

        public bool TryGetReward(out int rewardCount)
        {
            rewardCount = 0;

            if (_canReceive == false)
                return false;
            
            if (_rewardCount <= rewardCount)
                return false;
            
            rewardCount = _rewardCount;
            _rewardCount = 0;
            _canReceive = false;
            return true;
        }
    }
}