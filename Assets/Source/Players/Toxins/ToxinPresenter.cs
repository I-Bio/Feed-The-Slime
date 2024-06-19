namespace Players
{
    public class ToxinPresenter
    {
        private readonly PlayerToxins Model;
        private readonly ToxinBar Bar;
        private readonly Ticker Ticker;
        private readonly IPlayerVisitor Visitor;
        private readonly IRevival Revival;

        public ToxinPresenter(PlayerToxins model, ToxinBar bar, Ticker ticker, IPlayerVisitor visitor, IRevival revival)
        {
            Model = model;
            Bar = bar;
            Ticker = ticker;
            Visitor = visitor;
            Revival = revival;
        }

        public void Enable()
        {
            Model.Changed += OnChanged;
            Model.GoingDie += OnGoingDie;

            Bar.Hid += OnHid;
            Ticker.Ticked += OnTicked;
            Visitor.ToxinContacted += OnToxinContacted;
            Visitor.ContactStopped += OnContactStopped;
            Revival.Revived += OnRevived;
        }

        public void Disable()
        {
            Model.Changed -= OnChanged;
            Model.GoingDie -= OnGoingDie;

            Bar.Hid -= OnHid;
            Ticker.Ticked -= OnTicked;
            Visitor.ToxinContacted -= OnToxinContacted;
            Visitor.ContactStopped -= OnContactStopped;
            Revival.Revived -= OnRevived;
        }

        private void OnChanged(int value)
        {
            Bar.ChangeValue(value);
        }

        private void OnGoingDie()
        {
            Visitor.Visit(null, SatietyStage.Overeat);
        }

        private void OnHid()
        {
            Ticker.Stop();
        }

        private void OnTicked()
        {
            Model.Decrease();
        }

        private void OnToxinContacted()
        {
            Model.Increase();
            Ticker.Stop();
        }

        private void OnContactStopped()
        {
            Ticker.StartTick();
        }

        private void OnRevived()
        {
            Model.Drop();
        }
    }
}