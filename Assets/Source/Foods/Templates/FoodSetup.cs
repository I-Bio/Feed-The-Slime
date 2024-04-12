using System;
using Spawners;
using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(EdiblePart))]
    [RequireComponent(typeof(ObjectHighlighter))]
    public class FoodSetup : SpawnableObject
    {
        [SerializeField] private float _deselectValue = 0f;
        [SerializeField] private float _selectValue = 4f;
        [SerializeField] private float _scorePerEat;
        [SerializeField] private SatietyStage _stage;

        private EdiblePart _ediblePart;
        private ObjectHighlighter _highlighter;
            
        private Food _model;
        private FoodPresenter _presenter;

        private Action _onDestroyCallback;

        public SatietyStage Stage => _stage;
        
        public void Initialize(float scorePerEat, Action onDestroyCallback = null)
        {
            _ediblePart = GetComponent<EdiblePart>();
            _highlighter = GetComponent<ObjectHighlighter>();
            _onDestroyCallback = onDestroyCallback;

            _model = new Food(_stage);
            _presenter = new FoodPresenter(_model, _ediblePart, _highlighter);
            
            _ediblePart.Initialize(float.IsNaN(scorePerEat) ? _scorePerEat : scorePerEat);
            _highlighter.Initialize(_deselectValue, _selectValue);
            
            _presenter.Enable();
        }
        
        private void Start()
        {
            _highlighter.Deselect();
        }

        private void OnDestroy()
        {
            _presenter.Disable();
            _onDestroyCallback?.Invoke();
        }
    }
}