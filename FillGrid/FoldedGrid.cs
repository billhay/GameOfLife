namespace FillGrid
{
    using System;
    using System.Collections.Generic;

    public enum CellState
    {
        Dead = 0,
        Destroyed = 1,
        Created = 3,
        Alive = 4
    }

    public class FoldedGrid
    {
        private CellState[,] cells;
        private CellState[,] bcells;

        public FoldedGrid(int xMax, int yMax, IEnumerable<(int, int)> start)
        {
            this.XMax = xMax;
            this.YMax = yMax;
            this.cells = new CellState[xMax, yMax];
            this.bcells = new CellState[xMax, yMax];

            foreach ((int x, int y)  in start)
            {
                this[x, y] = CellState.Dead;
            }
        }

        public FoldedGrid(int xMax, int yMax)
            : this(xMax, yMax, new (int, int)[0])
        {
        }

        public int XMax { get; }

        public int YMax { get; }

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
                this.cells[x, y] = CellState.Dead;
                this.bcells[x, y] = CellState.Dead;
            });
        }

        public void Iterate()
        {
            this.ForEach((x, y) =>
            {
                int neighborCount = this.NeighborCount(x, y);
                var nextState = this.NewState(this.cells[x, y], neighborCount);
                this.bcells[x, y] = nextState;
            });

            var temp = this.bcells;
            this.bcells = this.cells;
            this.cells = temp;
        }

        public int NeighborCount(int x, int y)
        {
            int X(int xx, int yy)
            {
                var cstate = this[xx, yy];
                return cstate == CellState.Alive || cstate == CellState.Created ? 1 : 0;
            }

            var a = X(x - 1, y - 1);
            var b = X(x - 1, y);
            var c = X(x - 1, y + 1);
            var d = X(x, y - 1);
            var e = X(x, y + 1);
            var f = X(x + 1, y - 1);
            var g = X(x + 1, y);
            var h = X(x + 1, y + 1);

            int count = a + b + c + d + e + f + g + h;
            return count;
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
            for (int y = 0; y < this.YMax; y++)
            {
                for (int x = 0; x < this.XMax; x++)
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
                x = Wrap(x, this.XMax);
                y = Wrap(y, this.YMax);
                return this.cells[x, y];
            }

            set
            {
                if (value == CellState.Created  || value == CellState.Dead)
                {
                    x = Wrap(x, this.XMax);
                    y = Wrap(y, this.YMax);
                    int n = x + y * this.XMax;
                    this.cells[x, y] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid cell state");
                }
            }
        }

        private void ForEach(Action<int,int> action)
        {
            for (int y = 0; y < this.YMax; y++)
            {
                for (int x = 0; x < this.XMax; x++)
                {
                    action(x, y);
                }
            }
        }
    }
}
