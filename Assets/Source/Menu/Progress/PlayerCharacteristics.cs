using System;
using UnityEngine;

namespace Menu
{
    [Serializable]
    public class PlayerCharacteristics : IReadOnlyCharacteristics
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _scorePerEat;
        [SerializeField] private int _lifeCount;
        [SerializeField] private int _crystalsCount;
        [SerializeField] private int _completedLevels;
        [SerializeField] private bool _spitObtained;

        public float Speed { get => _speed; set => _speed = value; }
        public float ScorePerEat { get => _scorePerEat; set => _scorePerEat = value; }
        public int LifeCount { get => _lifeCount; set => _lifeCount = value; }
        public int CrystalsCount { get => _crystalsCount; set => _crystalsCount = value; }
        public int CompletedLevels { get => _completedLevels; set => _completedLevels = value; }
        public bool SpitObtained { get => _spitObtained; set => _spitObtained = value; }
    }
}