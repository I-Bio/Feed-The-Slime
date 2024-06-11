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

        public Booster Initialize(IStat boost)
        {
            _boost = boost;
            _text.SetText(boost.ToString());
            _renderer.sprite = _boost.Icon;
            return this;
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