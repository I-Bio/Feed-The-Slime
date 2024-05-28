using Enemies;
using Players;
using UnityEngine;

namespace Spawners
{
    public class CenterPreparer : ThemePreparer
    {
        [SerializeField] private Material _material;

        public void Initialize(IHidden player, IPlayerVisitor visitor, Renderer ground)
        {
            ground.material = _material;
            base.Initialize(player, visitor);
        }
    }
}