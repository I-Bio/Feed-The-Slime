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
        
        private IStatBuffer _boost;

        public void Initialize(KeyValuePair<IStatBuffer, KeyValuePair<string, Sprite>> boostValues)
        {
            _boost = boostValues.Key;
            _text.SetText(boostValues.Value.Key);
            _renderer.sprite = boostValues.Value.Value;
        }

        public void Use()
        {
            Push();
        }
        
        public IStatBuffer GetBoost()
        {
            return _boost; 
        }
    }
}