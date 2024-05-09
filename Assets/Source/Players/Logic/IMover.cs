namespace Players
{
    public interface IMover
    {
        public void AllowMove();
        public void ProhibitMove();

        public void Scale(SatietyStage stage);
    }
}