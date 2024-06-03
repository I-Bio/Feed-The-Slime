using UnityEngine;

public abstract class Contactable : MonoBehaviour
{
    public abstract bool TryContact(Bounds bounds);
}