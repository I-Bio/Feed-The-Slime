namespace Foods
{
    public class FoodPresenter : IPresenter
    {
        private readonly Food _model;
        private readonly EdiblePart _ediblePart;
        private readonly ObjectHighlighter _highlighter;
        
        public FoodPresenter(Food model, EdiblePart ediblePart, ObjectHighlighter highlighter)
        {
            _model = model;
            _ediblePart = ediblePart;
            _highlighter = highlighter;
        }

        public void Enable()
        {
            _model.Highlighted += OnHighlighted;
            _model.Allowed += OnAllowed;

            _highlighter.GoingSelect += OnGoingSelect;
        }

        public void Disable()
        {
            _model.Highlighted -= OnHighlighted;
            _model.Allowed -= OnAllowed;
            
            _highlighter.GoingSelect -= OnGoingSelect;
        }

        private void OnHighlighted()
        {
            _highlighter.SetSelection();
        }

        private void OnAllowed()
        {
            _ediblePart.Allow();
        }

        private void OnGoingSelect(SatietyStage stage)
        {
            _model.CompareStage(stage);
        }
    }
}