﻿using UnityEngine;

namespace Spawners
{
    public class CenterPreparer : ThemePreparer
    {
        [SerializeField] private Material _material;

        public void Initialize(IHidden player, Renderer ground)
        {
            ground.material = _material;
            base.Initialize(player);
        }
    }
}