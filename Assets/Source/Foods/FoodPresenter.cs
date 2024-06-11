namespace Foods
{
    public class FoodPresenter
    {
        private readonly Food Model;
        private readonly EdiblePart EdiblePart;
        private readonly ObjectHighlighter Highlighter;

        public FoodPresenter(Food model, EdiblePart ediblePart, ObjectHighlighter highlighter)
        {
            Model = model;
            EdiblePart = ediblePart;
            Highlighter = highlighter;
        }

        public void Enable()
        {
            Model.Highlighted += OnHighlighted;
            Model.Allowed += OnAllowed;

            Highlighter.GoingSelect += OnGoingSelect;
        }

        public void Disable()
        {
            Model.Highlighted -= OnHighlighted;
            Model.Allowed -= OnAllowed;

            Highlighter.GoingSelect -= OnGoingSelect;
        }

        private void OnHighlighted()
        {
            Highlighter.SetSelection();
        }

        private void OnAllowed()
        {
            EdiblePart.Allow();
        }

        private void OnGoingSelect(SatietyStage stage)
        {
            Model.CompareStage(stage);
        }
    }
}