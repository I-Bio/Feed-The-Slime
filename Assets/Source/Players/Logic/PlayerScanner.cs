using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class PlayerScanner : MonoBehaviour
    {
        private List<ISelectable> _selectables;
        private SatietyStage _stage;
        private Transform _transform;
        private float _startDistance;
        private float _distance;
        private float _scaleFactor;
        private bool _didInitialize;

        private void FixedUpdate() => Scan();
        
        public void Initialize(List<ISelectable> selectables, SatietyStage stage, Transform transform,
            float distance, float scaleFactor)
        {
            _selectables = selectables;
            _stage = stage;
            _transform = transform;
            _startDistance = distance;
            _distance = _startDistance;
            _scaleFactor = scaleFactor;
            _didInitialize = true;
        }

        public void SetStage(SatietyStage stage)
        {
            _stage = stage;
            _distance = _stage != SatietyStage.Exhaustion ? _startDistance * (_scaleFactor * (int)stage) : _startDistance;
        }

        public void AddSelectable(ISelectable selectable)
        {
            if (_selectables.Contains(selectable))
                return;
            
            _selectables.Add(selectable);
        }

        private void Scan()
        {
            if (_didInitialize == false)
                return;
            
            foreach (ISelectable selectable in _selectables)
            {
                if (selectable.Equals(null))
                {
                    _selectables.Remove(null);
                    continue;
                }
                
                if (Vector3.Distance(_transform.position, selectable.Position) <= _distance)
                {
                    selectable.Select(_stage);
                    continue;
                }
                
                selectable.Deselect();
            }
        }
    }
}