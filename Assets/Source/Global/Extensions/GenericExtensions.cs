using System.Collections.Generic;
using UnityEngine;

public static class GenericExtensions
{
    private const int MinValue = 0;

    public static T GetRandom<T>(this T[] array)
    {
        return array[Random.Range(MinValue, array.Length)];
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(MinValue, list.Count)];
    }
}