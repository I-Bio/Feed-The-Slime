using UnityEngine;

public interface ISelectable
{
    public Vector3 Position { get; }
    
    public void Select(SatietyStage playerStage);
    public void Deselect();
}