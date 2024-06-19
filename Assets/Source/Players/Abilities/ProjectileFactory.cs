using System;
using Spawners;
using UnityEngine;

namespace Players
{
    public class ProjectileFactory : IFactory<Projectile>
    {
        private readonly float CastStrength;
        private readonly Vector3 CastOffset;
        private readonly Transform CastPoint;
        private readonly IEatableFactory Spawner;
        private readonly Func<Vector3, Projectile> Pulling;

        public ProjectileFactory(
            float castStrength,
            Vector3 castOffset,
            Transform castPoint,
            IEatableFactory spawner,
            Func<Vector3, Projectile> pulling)
        {
            CastStrength = castStrength;
            CastOffset = castOffset;
            CastPoint = castPoint;
            Spawner = spawner;
            Pulling = pulling;
        }

        public Projectile Create()
        {
            return Pulling?.Invoke(CastPoint.position + CastOffset)
                .Initialize(CastStrength * CastPoint.forward, Spawner);
        }
    }
}