using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadeObjectBlockingObject : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _delay = 0.01f;

    private readonly List<FadingObject> _objectsBlocking = new List<FadingObject>();
    private readonly RaycastHit[] _hits = new RaycastHit[10];

    private void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        var wait = new WaitForSeconds(_delay);

        while (true)
        {
            Vector3 cameraPosition = _camera.position;
            Vector3 playerPosition = _player.position;
            
            int hits = Physics.RaycastNonAlloc(cameraPosition, (playerPosition - cameraPosition).normalized, _hits,
                Vector3.Distance(cameraPosition, playerPosition), _layerMask);
            
            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(_hits[i]);

                    if (fadingObject == null || _objectsBlocking.Contains(fadingObject) == true)
                        continue;
                    
                    _objectsBlocking.Add(fadingObject);
                    fadingObject.FadeOut();
                }
            }

            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return wait;
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        for (int i = 0; i < _objectsBlocking.Count; i++)
        {
            bool objectIsBeingHit = _hits.Select(GetFadingObjectFromHit)
                .Any(fadingObject => fadingObject != null && fadingObject == _objectsBlocking[i]);

            if (objectIsBeingHit == true) 
                continue;
            
            if (_objectsBlocking[i] != null)
                _objectsBlocking[i].FadeIn();
            
            _objectsBlocking.RemoveAt(i);
        }
    }

    private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
    {
        return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
    }

    private void ClearHits()
    {
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < _hits.Length; i++)
            _hits[i] = hit;
    }
}