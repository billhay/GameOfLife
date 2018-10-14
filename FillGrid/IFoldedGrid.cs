namespace FillGrid
{
    public interface IFoldedGrid
    {
        ICell this[int x, int y] { get; }

        void Reset();

        void Iterate();
    }
}