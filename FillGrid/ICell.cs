//-----------------------------------------------------------------------
// <copyright file="ICell.cs" company="Bill">
//     Copyright (C) 2014 Bill Hay  All rights reserved.
// </copyright>
// <summary>The ICell interface</summary>
//-----------------------------------------------------------------------

namespace FillGrid
{
    /// <summary>
    /// Allowed states for cell
    /// </summary>
    public enum CellState
    {
        NotSet = 0,
        Dead = 1,
        Destroyed = 2,
        Created = 3,
        Alive = 4
    }

    /// <summary>
    /// Public interface for a cell
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets or sets the state of the cell (dead, alive ...)
        /// </summary>
        CellState State { get; set; }
    }
}