using System.Collections.Generic;
using Spawners;
using UnityEngine;

namespace Menu
{
    public class LeaderboardView : ObjectPool
    {
        private readonly List<LeaderboardElement> Spawned = new ();

        private Transform _container;

        public void Initialize(Transform container, LeaderboardElement element)
        {
            Initialize(element);
            _container = container;
        }

        public void Construct(List<LeaderboardPlayer> players)
        {
            Clear();

            foreach (LeaderboardPlayer player in players)
            {
                Spawned.Add(
                    Pull<LeaderboardElement>(_container)
                        .Initialize(player.Rank, player.Name, player.Score));
            }
        }

        private void Clear()
        {
            foreach (LeaderboardElement element in Spawned)
                element.Push();

            Spawned.Clear();
        }
    }
}