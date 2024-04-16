﻿namespace Menu
{
    public interface ITransferService
    {
        public IReadOnlyCharacteristics Characteristics { get; }
        public int Reward { get; }

        public void AllowReceive();
        public void PassLevel();
        public void MultiplyIt(float value);
    }
}