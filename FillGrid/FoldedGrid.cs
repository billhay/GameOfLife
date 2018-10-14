namespace FillGrid
{
    using System;
    using System.Collections.Generic;


    public class FoldedGrid
    {
        private readonly Cell[,] cells;
        private readonly int xMax;
        private readonly int yMax;

        public FoldedGrid(int xMax, int yMax, IEnumerable<(int, int)> start)
        {
            this.xMax = xMax;
            this.yMax = yMax;
            this.cells = new Cell[xMax, yMax];

            this.ForEach((x, y) =>
            {
                this.cells[x, y] = new Cell { State = CellState.Dead };
            });

            this.ForEach((x, y) =>
            {
                this.cells[x, y].Neighbors = this.GetNeighbors(x, y);
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

        public static int Wrap(int n, int nMax)
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

        public void Clear()
        {
            this.ForEach((x, y) =>
            {
                this.cells[x, y].Reset();
            });
        }

        public void Iterate()
        {
            this.ForEach((x, y) =>
            {
                int neighborCount = this.GetNeighborCount(this.cells[x,y]);
                var nextState = this.NewState(this.cells[x, y].State, neighborCount);
                this.cells[x, y].TempState = nextState;
            });

            this.ForEach((x ,y) => this.cells[x,y].FlipStates());
        }

        private Cell[] GetNeighbors(int x, int y)
        {
            Cell[] neighbors = new Cell[8];
            neighbors[0] = this.GetCell(x - 1, y - 1);
            neighbors[1] = this.GetCell(x - 1, y);
            neighbors[2] = this.GetCell(x - 1, y + 1);
            neighbors[3] = this.GetCell(x, y - 1);
            neighbors[4] = this.GetCell(x, y + 1);
            neighbors[5] = this.GetCell(x + 1, y - 1);
            neighbors[6] = this.GetCell(x + 1, y);
            neighbors[7] = this.GetCell(x + 1, y + 1);
            return neighbors;
        }

        public int GetNeighborCount(Cell c)
        {
            int n = 0;
            foreach (Cell neighbor in c.Neighbors)
            {
                n = n + (neighbor.State == CellState.Alive || neighbor.State == CellState.Created ? 1 : 0);
            }

            return n;
        }


        /// <summary>
        /// returns new state given old state and number of live neighbors
        /// </summary>
        /// <param name="oldState">The current state</param>
        /// <param name="neighborCount">The current number of neighbors</param>
        /// <returns>The new state</returns>
        /// <remarks>See: https://en.wikipedia.org/w/index.php?title=Conway%27s_Game_of_Life </remarks>
        public CellState NewState(CellState oldState, int neighborCount)
        {
                var foo = oldState == CellState.Alive || oldState == CellState.Created 
                ? neighborCount < 2 || neighborCount > 3 ? CellState.Destroyed : CellState.Alive
                : neighborCount == 3 ? CellState.Created : CellState.Dead;
            return foo;
        }

        public void Dump()
        {
            for (int y = 0; y < this.yMax; y++)
            {
                for (int x = 0; x < this.xMax; x++)
                {
                    Console.Write(this[x,y] == CellState.Alive ? " * " : "   ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public CellState this[int x, int y]
        {

            get
            {
                x = Wrap(x, this.xMax);
                y = Wrap(y, this.yMax);
                return this.cells[x, y].State;
            }

            set
            {
                if (value == CellState.Created  || value == CellState.Dead)
                {
                    x = Wrap(x, this.xMax);
                    y = Wrap(y, this.yMax);
                    int n = x + y * this.xMax;
                    this.cells[x, y].State = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid cell state");
                }
            }
        }

        private Cell GetCell(int x, int y)
        {
            x = Wrap(x, this.xMax);
            y = Wrap(y, this.yMax);
            return this.cells[x, y];
        }

        private void ForEach(Action<int,int> action)
        {
            for (int y = 0; y < this.yMax; y++)
            {
                for (int x = 0; x < this.xMax; x++)
                {
                    action(x, y);
                }
            }
        }
    }
}
