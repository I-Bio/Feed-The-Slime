using System.Collections.Generic;
using Spawners;
using TMPro;
using UnityEngine;

namespace Boosters
{
    public class Booster : SpawnableObject, IBooster
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private SpriteRenderer _renderer;
        
        private IStat _boost;

        public void Initialize(IStat boost)
        {
            _boost = boost;
            _text.SetText(boost.ToString());
            _renderer.sprite = _boost.Icon;
        }

        public void Use()
        {
            Push();
        }
        
        public IStat GetBoost()
        {
            return _boost; 
        }
    }
}