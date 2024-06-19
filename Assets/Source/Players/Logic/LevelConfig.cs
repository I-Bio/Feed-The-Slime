using UnityEngine;

namespace Players
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Player/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private float _scaleFactor = 2.5f;
        [SerializeField] private float _cameraScale = 2f;
        [SerializeField] private float _scoreScaler = 2f;
        [SerializeField] private float _startScore = 0f;
        [SerializeField] private int _startMaxScore = 6;
        [SerializeField] private int _levelsPerStage = 3;
        [SerializeField] private SatietyStage _startStage = SatietyStage.Exhaustion;
        [SerializeField] private int _minToxinsCount = 0;
        [SerializeField] private int _maxToxinsCount = 100;

        public float ScaleFactor => _scaleFactor;

        public float CameraScale => _cameraScale;

        public float ScoreScaler => _scoreScaler;

        public float StartScore => _startScore;

        public int StartMaxScore => _startMaxScore;

        public int LevelsPerStage => _levelsPerStage;

        public SatietyStage StartStage => _startStage;

        public int MinToxinsCount => _minToxinsCount;

        public int MaxToxinsCount => _maxToxinsCount;
    }
}