namespace FillGrid
{
    public interface IFoldedGrid
    {
        void Reset();
        void Iterate();
        ICell this[int x, int y] { get; }
    }
}