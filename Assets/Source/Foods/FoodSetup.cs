using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(EdiblePart))]
    [RequireComponent(typeof(ObjectHighlighter))]
    public class FoodSetup : MonoBehaviour
    {
        [SerializeField] private Material _standard;
        [SerializeField] private Material _highlighted;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _scorePerEat = 0.6f;
        [SerializeField] private SatietyStage _stage;

        private EdiblePart _ediblePart;
        private ObjectHighlighter _highlighter;
            
        private Food _model;
        private FoodPresenter _presenter;
        
        public void Initialize()
        {
            _ediblePart = GetComponent<EdiblePart>();
            _highlighter = GetComponent<ObjectHighlighter>();
            
            _model = new Food(_stage);
            _presenter = new FoodPresenter(_model, _ediblePart, _highlighter);
            
            _ediblePart.Initialize(_scorePerEat);
            _highlighter.Initialize(_meshRenderer, _standard, _highlighted);
        }

        private void OnEnable()
        {
            _presenter.Enable();
        }

        private void OnDisable()
        {
            _presenter.Disable();
        }
    }
}