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

    public interface ICell
    {
        CellState State { get; set; }
    }
}