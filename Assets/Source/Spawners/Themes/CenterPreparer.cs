using System.Collections.Generic;
using Players;
using UnityEngine;

namespace Spawners
{
    public class CenterPreparer : ThemePreparer
    {
        [SerializeField] private Material _material;

        public List<Contactable> Initialize(IHidden player, IPlayerVisitor visitor, out List<ISelectable> selectables, Renderer ground)
        {
            ground.material = _material;
            return base.Initialize(player, visitor, out selectables);
        }
    }
}