using UnityEngine;

public interface IHidden
{
    public bool IsHidden { get; }

    public Vector3 Position { get; }

    public SatietyStage Stage { get; }
}