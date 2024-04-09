using UnityEngine;

namespace Players
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Player/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] public float ScaleFactor;
        [SerializeField] public float CameraScale;
        [SerializeField] public float ScoreScaler;
        [SerializeField] public float StartScore = 0f;
        [SerializeField] public int StartMaxScore;
        [SerializeField] public int LevelsPerStage;
        [SerializeField] public SatietyStage StartStage;
    }
}