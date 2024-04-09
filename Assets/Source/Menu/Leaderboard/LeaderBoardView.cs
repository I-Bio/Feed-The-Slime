using System.Collections.Generic;
using Spawners;
using UnityEngine;

namespace Menu
{
    public class LeaderBoardView : ObjectPool
    {
        [SerializeField] private Transform _container;

        private readonly List<LeaderboardElement> _spawned = new();

        public void Construct(List<LeaderboardPlayer> players)
        {
            Clear();

            foreach (LeaderboardPlayer player in players)
                _spawned.Add(PullAndSetParent<LeaderboardElement>(Vector3.zero, _container)
                    .Initialize(player.Rank, player.Name, player.Score));
        }

        private void Clear()
        {
            foreach (LeaderboardElement element in _spawned)
                element.Push();

            _spawned.Clear();
        }
    }
}