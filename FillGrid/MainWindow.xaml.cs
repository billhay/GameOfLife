﻿namespace FillGrid
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int CellSize = 10;

        private static readonly Brush BrushDead = Brushes.Black;
        private static readonly Brush BrushDestroyed = Brushes.DimGray;
        private static readonly Brush BrushAlive = Brushes.White;
        private static readonly Brush BrushCreated = Brushes.NavajoWhite;

        private static readonly Dictionary<CellState, Brush> Dict1 =
            new Dictionary<CellState, Brush>
            {
                {CellState.Dead, BrushDead},
                {CellState.Destroyed, BrushDead},
                {CellState.Alive, BrushAlive},
                {CellState.Created, BrushAlive}
            };

        private static readonly Dictionary<CellState, Brush> Dict2 =
            new Dictionary<CellState, Brush>
            {
                {CellState.Dead, BrushDead},
                {CellState.Destroyed, BrushDestroyed},
                {CellState.Alive, BrushAlive},
                {CellState.Created, BrushCreated}
            };

        private readonly FoldedGrid grid;

        private readonly Rectangle[,] rectangles;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly int xMax;
        private readonly int yMax;

        private bool showCreateAndDestroy;

        public MainWindow()
        {
            this.InitializeComponent();

            this.xMax = (int) this.myCanvas.Width;
            this.yMax = (int) this.myCanvas.Height;
            this.xMax = this.xMax / CellSize;
            this.yMax = this.yMax / CellSize;
            this.myCanvas.Width = CellSize * this.xMax;
            this.myCanvas.Height = CellSize * this.yMax;
            this.rectangles = new Rectangle[this.xMax, this.yMax];
            this.Stop.IsEnabled = false;
            this.timer.IsEnabled = false;
            this.timer.Interval = TimeSpan.FromSeconds(0.1);
            this.timer.Tick += this.OnTimer;
            this.grid = new FoldedGrid(this.xMax, this.yMax);

            this.ForEach((x, y) =>
            {
                Rectangle r = new Rectangle
                {
                    Tag = (x, y),
                    Width = CellSize,
                    Height = CellSize,
                    Fill = BrushDead
                };

                var foo = r.Tag;
                var baz = foo as Tuple<int, int>;

                r.MouseLeftButtonDown += this.OnSelectInitialCell;
                Canvas.SetTop(r, CellSize * y);
                Canvas.SetLeft(r, CellSize * x);
                this.myCanvas.Children.Add(r);
                this.rectangles[x, y] = r;
            });
        }

        private void OnSelectInitialCell(object sender, MouseButtonEventArgs e)
        {
            if (this.Iterate.IsEnabled)
            {
                Rectangle r = (Rectangle) sender;
                (int x, int y) = (ValueTuple<int, int>) r.Tag;

                if (this.grid[x, y] == CellState.Dead)
                {
                    this.grid[x, y] = CellState.Created;
                    r.Fill = this.GetBrush(CellState.Created);
                }
                else
                {
                    this.grid[x, y] = CellState.Dead;
                    r.Fill = this.GetBrush(CellState.Dead);
                }
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.NextIteration();
            this.timer.Interval = TimeSpan.FromMilliseconds(1000 - 90 * this.Speed.Value);
            this.timer.Start();
        }

        private void Iterate_Click(object sender, RoutedEventArgs e)
        {
            this.NextIteration();
        }

        private void NextIteration()
        {
            this.grid.Iterate();
            this.ForEach((x, y) => { this.rectangles[x, y].Fill = this.GetBrush(this.grid[x, y]); });
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            this.Run.IsEnabled = false;
            this.Iterate.IsEnabled = false;
            this.Stop.IsEnabled = true;
            this.Clear.IsEnabled = false;
            this.ShowChanges.IsEnabled = false;
            this.timer.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            this.Run.IsEnabled = true;
            this.Iterate.IsEnabled = true;
            this.Stop.IsEnabled = false;
            this.Clear.IsEnabled = true;
            this.ShowChanges.IsEnabled = true;
        }

        private Brush GetBrush(CellState state)
        {
            Dictionary<CellState, Brush> dict = this.showCreateAndDestroy ? Dict2 : Dict1;
            return dict[state];
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            this.showCreateAndDestroy = true;
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            this.showCreateAndDestroy = false;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.grid.Clear();
            this.ForEach((x,y) => this.rectangles[x, y].Fill = BrushDead);
        }

        private void ForEach(Action<int, int> action)
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