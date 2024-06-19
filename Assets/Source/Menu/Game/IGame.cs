namespace Menu
{
    public interface IGame
    {
        public void SetStage(SatietyStage stage);

        public void Win();

        public void Lose();
    }
}