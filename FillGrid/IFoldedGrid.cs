// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFoldedGrid.cs" company="Bill">
//   Bill Hay
// </copyright>
// <summary>
//   Defines the IFoldedGrid type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FillGrid
{
    /// <summary>
    /// External interface into Folded Grid
    /// </summary>
    public interface IFoldedGrid
    {
        /// <summary>
        /// returns the cell at the specified location in the grid
        /// </summary>
        /// <param name="x">The x parameter</param>
        /// <param name="y">The y parameter</param>
        /// <returns>The cell</returns>
        ICell this[int x, int y] { get; }

        /// <summary>
        /// Sets the state of all cells in the grid to DEAD
        /// </summary>
        void Reset();

        /// <summary>
        /// Run one iteration of the cellular automaton
        /// </summary>
        void Iterate();
    }
}