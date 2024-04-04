using System;
using System.Collections.Generic;

[Serializable]
public struct SerializedPair<T>
{
    public int Key;
    public T Value;
        
    public static bool operator ==(SerializedPair<T> first, SerializedPair<T> second) 
    {
        return first.Equals(second);
    }

    public static bool operator !=(SerializedPair<T> first, SerializedPair<T> second) 
    {
        return !first.Equals(second);
    }
    
    public bool Equals(SerializedPair<T> other)
    {
        return Key == other.Key && EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object @object)
    {
        return @object is SerializedPair<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value);
    }
}