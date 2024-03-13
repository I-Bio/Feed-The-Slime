using System;
using System.Collections;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _knob;
    [SerializeField] private RectTransform _holderTransform;
    [SerializeField] private GameObject _holder;
        
    private Coroutine _coroutine;

    public event Action Released;

    private void Start()
    {
        Release();
    }

    public void Activate(Vector2 position)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, position, _camera,
                out Vector2 localPosition) == false)
            return;

        _holderTransform.anchoredPosition = localPosition;
        _holder.SetActive(true);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
            
        _coroutine = StartCoroutine(ReleaseRoutine());
    }

    private void Release()
    {
        Released?.Invoke();
        _holder.SetActive(false);
    }

    private IEnumerator ReleaseRoutine()
    {
        Vector2 lastPosition = Vector2.zero;

        while (_knob.anchoredPosition != Vector2.zero || lastPosition == Vector2.zero)
        {
            lastPosition = _knob.anchoredPosition;
            yield return null;
        }
            
        Release();
    }
}