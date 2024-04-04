namespace Menu
{
    public interface ITransferService
    {
        public IReadOnlyCharacteristics Characteristics { get; }

        public void AllowReceive();
        public void MultiplyIt(float value);
    }
}