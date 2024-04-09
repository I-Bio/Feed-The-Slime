using System.Collections.Generic;
using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

namespace Menu
{
    public class YandexLeaderboard : LeanLocalizedBehaviour
    {
        private readonly List<LeaderboardPlayer> _players = new();

        [SerializeField] private string _leaderboardName;
        [SerializeField] private string _anonymousName;
        [SerializeField] private LeaderBoardView _leaderBoard;
        
        public override void UpdateTranslation(LeanTranslation translation)
        {
            if (translation == null)
                return;
            
            if (translation.Data is string == false)
                return;
            
            _anonymousName = translation.Data as string;
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

            _players.Clear();

            Leaderboard.GetEntries(_leaderboardName, (result) =>
            {
                foreach (var entry in result.entries)
                    _players.Add(new LeaderboardPlayer(
                        entry.rank,
                        entry.player.publicName ?? _anonymousName,
                        entry.score));

                _leaderBoard.Construct(_players);
            });
        }
    }
}