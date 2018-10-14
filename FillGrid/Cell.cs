namespace FillGrid
{
    using System;

    public class Cell : ICell
    {
        public CellState State { get; set; }

        public CellState TempState { get; set; }

        public Func<int> LiveNeighborCount { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public void FlipStates()
        {
            this.State = this.TempState;
            this.TempState = CellState.NotSet;
        }

        public void Reset()
        {
            this.State = CellState.Dead;
            this.TempState = CellState.Dead;
        }
    }
}