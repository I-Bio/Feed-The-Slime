using UnityEngine;

namespace Foods
{
    [RequireComponent(typeof(EdiblePart))]
    [RequireComponent(typeof(ObjectHighlighter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class FoodSetup : MonoBehaviour
    {
        [SerializeField] private Material[] _standard;
        [SerializeField] private Material[] _highlighted;
        [SerializeField] private float _scorePerEat;
        [SerializeField] private SatietyStage _stage;

        private EdiblePart _ediblePart;
        private ObjectHighlighter _highlighter;
        private MeshRenderer _meshRenderer;
            
        private Food _model;
        private FoodPresenter _presenter;
        
        public void Initialize()
        {
            _ediblePart = GetComponent<EdiblePart>();
            _highlighter = GetComponent<ObjectHighlighter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            
            _model = new Food(_stage);
            _presenter = new FoodPresenter(_model, _ediblePart, _highlighter);
            
            _ediblePart.Initialize(_scorePerEat);
            _highlighter.Initialize(_meshRenderer, _standard, _highlighted);
            
            _presenter.Enable();
        }
        
        private void Start()
        {
            _highlighter.Deselect();
        }

        private void OnDestroy()
        {
            _presenter.Disable();
        }
    }
}