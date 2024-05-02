using System.Collections.Generic;
using Agava.YandexGames;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class YandexLeaderboard : MonoBehaviour
    {
        private readonly List<LeaderboardPlayer> Players = new();

        private LeaderboardView _leaderboard;
        private string _leaderboardName;
        private string _anonymousName;

        public void Initialize(LeaderboardView leaderboard, string leaderboardName,
            string anonymousName, Transform container, LeaderboardElement element)
        {
            _leaderboard = leaderboard;
            _leaderboardName = leaderboardName;
            _anonymousName = anonymousName;
            _leaderboard.Initialize(container, element);
        }

        public void SetPlayerScore(int score)
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(_leaderboardName, (result) =>
            {
                if (result == null || result.score < score)
                    Leaderboard.SetScore(_leaderboardName, score);
            });
        }

        public void Fill()
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Players.Clear();

            Leaderboard.GetEntries(_leaderboardName, (result) =>
            {
                foreach (var entry in result.entries)
                    Players.Add(new LeaderboardPlayer(
                        entry.rank,
                        entry.player.publicName ?? _anonymousName,
                        entry.score));

                _leaderboard.Construct(Players);
            });
        }
    }
}