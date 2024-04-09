using Spawners;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class LeaderboardElement : SpawnableObject
    {
        [SerializeField] private TextMeshProUGUI _playerRank;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _playerScore;

        public LeaderboardElement Initialize(int rank, string name, int score)
        {
            _playerRank.SetText(rank.ToString());
            _playerName.SetText(name);
            _playerScore.SetText(score.ToString());
            return this;
        }
    }
}