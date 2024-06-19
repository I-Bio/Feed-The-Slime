using Spawners;
using UnityEngine;

namespace Menu
{
    public class RewardGem : SpawnableObject
    {
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;
    }
}