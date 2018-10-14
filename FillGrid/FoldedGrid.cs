namespace FillGrid
{
    using System;
    using System.Collections.Generic;

    public class FoldedGrid : IFoldedGrid
    {
        private readonly Cell[,] cells;
        private readonly int xMax;
        private readonly int yMax;

        public FoldedGrid(int xMax, int yMax, IEnumerable<(int, int)> start)
        {
            this.xMax = xMax;
            this.yMax = yMax;
            this.cells = CreateCellMatrix(xMax, yMax);

            this.ForEach((cell) =>
            {
                cell.LiveNeighborCount = this.NeighborCounter(cell);
            });

            foreach ((int x, int y)  in start)
            {
                this.cells[x, y].State = CellState.Created;
            }
        }

        public FoldedGrid(int xMax, int yMax)
            : this(xMax, yMax, new (int, int)[0])
        {
        }

        public ICell this[int x, int y]
        {
            get
            {
                x = Wrap(x, this.xMax);
                y = Wrap(y, this.yMax);
                return this.cells[x, y];
            }
        }

        public void Reset()
        {
            this.ForEach((cell) => cell.Reset());
        }

        public void Iterate()
        {
            this.ForEach((cell) =>
            {
                int neighborCount = cell.LiveNeighborCount();
                cell.TempState = this.NewState(cell.State, neighborCount);
            });

            this.ForEach((cell) => cell.FlipStates());
        }

        private static int Wrap(int n, int nMax)
        {
            if (n == -1)
            {
                return nMax - 1;
            }

            if (n == nMax)
            {
                return 0;
            }

            if (n >= 0 && n < nMax)
            {
                return n;
            }

            throw new IndexOutOfRangeException($"index out of range, it is {n}, it must be between -1 and {nMax}");
        }

        private Func<int> NeighborCounter(Cell cell)
        { 
            int x = cell.X;
            int y = cell.Y;

            ICell[] neighbors = new ICell[8];
            neighbors[0] = this[x - 1, y - 1];
            neighbors[1] = this[x - 1, y];
            neighbors[2] = this[x - 1, y + 1];
            neighbors[3] = this[x, y - 1];
            neighbors[4] = this[x, y + 1];
            neighbors[5] = this[x + 1, y - 1];
            neighbors[6] = this[x + 1, y];
            neighbors[7] = this[x + 1, y + 1];

            return () =>
            {
                int n = 0;
                foreach (ICell neighbor in neighbors)
                {
                    n = n + (neighbor.State == CellState.Alive || neighbor.State == CellState.Created ? 1 : 0);
                }

                return n;
            };
        }

        /// <summary>
        /// returns new state given old state and number of live neighbors
        /// </summary>
        /// <param name="oldState">The current state</param>
        /// <param name="neighborCount">The current number of neighbors</param>
        /// <returns>The new state</returns>
        /// <remarks>See: https://en.wikipedia.org/w/index.php?title=Conway%27s_Game_of_Life </remarks>
        private CellState NewState(CellState oldState, int neighborCount)
        {
                var foo = oldState == CellState.Alive || oldState == CellState.Created 
                ? neighborCount < 2 || neighborCount > 3 ? CellState.Destroyed : CellState.Alive
                : neighborCount == 3 ? CellState.Created : CellState.Dead;
            return foo;
        }

        private static Cell[,] CreateCellMatrix(int xmax, int ymax)
        {
            Cell[,] cells = new Cell[xmax, ymax];

            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    cells[x, y] = new Cell { X = x, Y = y, State = CellState.Dead };
                }
            }

            return cells;
        }

        private void ForEach(Action<Cell> action)
        {
            foreach (Cell cell in this.cells)
            {
                action(cell);
            }
        }
    }
}
