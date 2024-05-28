using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadeCaster : MonoBehaviour
{
    private readonly List<ObjectFader> FadedObjects = new();
    
    private RaycastHit[] _hits;
    private LayerMask _layerMask;
    private Transform _player;
    private Transform _camera;
    private float _delay = 0.01f;

    public void Initialize(LayerMask layerMask, Transform player, Transform camera, float delay, int hitsCapacity)
    {
        _layerMask = layerMask;
        _player = player;
        _camera = camera;
        _delay = delay;
        _hits = new RaycastHit[hitsCapacity];
        
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
                    ObjectFader fadingObject = GetFadingFromHit(_hits[i]);

                    if (fadingObject == null || FadedObjects.Contains(fadingObject) == true)
                        continue;
                    
                    FadedObjects.Add(fadingObject);
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
        for (int i = 0; i < FadedObjects.Count; i++)
        {
            bool objectIsBeingHit = _hits.Select(GetFadingFromHit)
                .Any(fadingObject => fadingObject != null && fadingObject == FadedObjects[i]);

            if (objectIsBeingHit == true) 
                continue;
            
            if (FadedObjects[i] != null)
                FadedObjects[i].FadeIn();
            
            FadedObjects.RemoveAt(i);
        }
    }

    private ObjectFader GetFadingFromHit(RaycastHit hit)
    {
        return hit.collider != null ? hit.collider.GetComponent<ObjectFader>() : null;
    }

    private void ClearHits()
    {
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < _hits.Length; i++)
            _hits[i] = hit;
    }
}