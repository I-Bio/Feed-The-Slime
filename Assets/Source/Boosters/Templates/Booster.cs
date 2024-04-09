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

        public void Initialize(KeyValuePair<IStatBuffer, string> boostPair)
        {
            _boost = boostPair.Key;
            _text.SetText(boostPair.Value);
            _renderer.sprite = boostPair.Key.Icon;
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