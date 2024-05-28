using System;
using UnityEngine;

public class Revival : MonoBehaviour, IRevival
{
    private Transform _player;
    private Vector3 _startPosition;
    private int _maxLifeCount;
    private int _currentLifeCount;

    public event Action Revived;

    public void Initialize(Transform player, int maxLifeCount)
    {
        _player = player;
        _startPosition = _player.position;
        _maxLifeCount = maxLifeCount;
    }

    public bool TryRevive()
    {
        if (_currentLifeCount >= _maxLifeCount)
            return false;

        _currentLifeCount++;
        Revive();
        return true;
    }

    public void Revive()
    {
        _player.position = _startPosition;
        Revived?.Invoke();
    }
}