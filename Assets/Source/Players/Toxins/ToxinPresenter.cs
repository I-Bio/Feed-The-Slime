using Enemies;

namespace Players
{
    public class ToxinPresenter : IPresenter
    {
        private readonly PlayerToxins _model;
        private readonly ToxinBar _bar;
        private readonly Ticker _ticker;
        private readonly IPlayerVisitor _visitor;
        
        public ToxinPresenter(PlayerToxins model, ToxinBar bar, Ticker ticker, IPlayerVisitor visitor)
        {
            _model = model;
            _bar = bar;
            _ticker = ticker;
            _visitor = visitor;
        }

        public void Enable()
        {
            _model.ToxinsChanged += OnToxinsChanged;
            _model.GoingDie += OnGoingDie;

            _ticker.Ticked += OnTicked;
            _visitor.ToxinContacted += OnToxinContacted;
            _visitor.ContactStopped += OnContactStopped;
        }

        public void Disable()
        {
            _model.ToxinsChanged -= OnToxinsChanged;
            _model.GoingDie -= OnGoingDie;

            _bar.Hid -= OnHid;
            _ticker.Ticked -= OnTicked;
            _visitor.ToxinContacted -= OnToxinContacted;
            _visitor.ContactStopped -= OnContactStopped;
        }

        private void OnToxinContacted()
        {
            _model.Increase();
            _ticker.Stop();
        }
        
        private void OnGoingDie()
        {
            _visitor.Visit(null, SatietyStage.Overeat);
        }

        private void OnHid()
        {
            _ticker.Stop();
        }
        
        private void OnTicked()
        {
            _model.Decrease();
        }

        private void OnToxinsChanged(int value)
        {
            _bar.ChangeValue(value);
        }

        private void OnContactStopped()
        {
            _ticker.StartTick();
        }
    }
}