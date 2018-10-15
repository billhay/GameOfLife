//-----------------------------------------------------------------------
// <copyright file="Cell.cs" company="Bill">
//     Copyright (C) 2014 Bill Hay  All rights reserved.
// </copyright>
// <summary>The Cell class</summary>
//-----------------------------------------------------------------------
namespace FillGrid
{
    using System;

    /// <summary>
    /// The Cell class
    /// </summary>
    public class Cell : ICell
    {
        /// <summary>
        /// Gets or sets the state of the cell (dead, alive ...)
        /// </summary>
        public CellState State { get; set; }

        /// <summary>
        /// Gets or sets the temporary state of the cell - used during iteration
        /// </summary>
        internal CellState TempState { get; set; }

        /// <summary>
        /// Gets or sets a Func that returns the count of live neighbors
        /// </summary>
        internal Func<int> LiveNeighborCount { get; set; }

        /// <summary>
        /// Gets or sets the column occupied by the cell (x value)
        /// </summary>
        internal int X { get; set; }

        /// <summary>
        /// Gets or sets the row occupied by the cell (y value)
        /// </summary>
        internal int Y { get; set; }

        internal void FlipStates()
        {
            this.State = this.TempState;
            this.TempState = CellState.NotSet;
        }

        internal void Reset()
        {
            this.State = CellState.Dead;
            this.TempState = CellState.Dead;
        }
    }
}