using System;
using System.Collections.Generic;
using Spawners;
using UnityEngine;

namespace Boosters
{
    public class IconFactory : IIconFactory
    {
        private readonly Transform IconHolder;
        private readonly Func<Transform, BoostIcon> Pulling;

        public IconFactory(Transform iconHolder, Func<Transform, BoostIcon> pullingIcon)
        {
            IconHolder = iconHolder;
            Pulling = pullingIcon;
        }

        public KeyValuePair<SpawnableObject, IStat> Create(IStat stat)
        {
            return new KeyValuePair<SpawnableObject, IStat>(
                Pulling?.Invoke(IconHolder).Initialize(stat.LifeTime, stat.Icon).Activate(),
                stat);
        }
    }
}