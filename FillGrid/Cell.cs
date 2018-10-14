namespace FillGrid
{
    public enum CellState
    {
        NotSet = 0,
        Dead = 1,
        Destroyed = 2,
        Created = 3,
        Alive = 4
    }

    public class Cell
    {
        public CellState State { get; set; }

        public CellState TempState { get; set; }

        public Cell[] Neighbors { get; set; }

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