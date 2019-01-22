using CodeProject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static CheckersUiWf.Boundary;

namespace CheckersUiWf
{
    public delegate void OnClickDelegate(int row, int col, int square);

    public struct Location
    {
        public int Row;
        public int Col;
    }
    public struct Swoop
    {
        public Location From;
        public Location To;
    }

    partial class Board
    {
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            IsDirty = false;
            Swoops = new Dictionary<int, Swoop>();
            CellWidth = width / BoardColumnCount;
            CellHeight = height / BoardRowCount;
            InitializeComponent();
        }

        public event OnClickDelegate OnSelected;

        public void NewGame()
        {
            for (int square = 1; square <= NumberSquares; square++)
            {
                if (square <= NumberEachTeam)
                {
                    SetSquare(square, CellState.Black, HighLight.None);
                }
                else if (square >= NumberSquares - NumberEachTeam + 1)
                {
                    SetSquare(square, CellState.White, HighLight.None);
                }
                else
                {
                    SetSquare(square, CellState.Empty, HighLight.None);
                }
            }
        }

        public void SetSquare(int square, CellState state, HighLight highlight)
        {
            if (square < 0 || square > NumberSquares) gCallBack.Panic("Invalid square number: " + square);
            Squares[square].CellState = state;
            Squares[square].HightLight = highlight;
            Squares[square].Refresh();
        }

        public void SetSquareState(int square, CellState state)
        {
            if (square < 0 || square > NumberSquares) gCallBack.Panic("Invalid square number: " + square);
            Squares[square].CellState = state;
            Squares[square].Refresh();
        }

        public void SetCellState(int row, int col, CellState state)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) gCallBack.Panic("Invalid cell index : " + row + ", " + col);
            Cells[row][col].CellState = state;
            Cells[row][col].Refresh();
        }

        public CellState GetSquareState(int square)
        {
            if (square < 0 || square > NumberSquares) gCallBack.Panic("Invalid square number: " + square);
            return Squares[square].CellState;
        }

        public CellState GetCellState(int row, int col)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) gCallBack.Panic("Invalid cell index : " + row + ", " + col);
            return Cells[row][col].CellState;
        }

        public void SetSquareHighLight (int square, HighLight highlight)
        {
            if (square < 0 || square > NumberSquares) gCallBack.Panic("Invalid square number: " + square);
            Squares[square].HightLight = highlight;
            Squares[square].Refresh();
        }

        public void SetCellHightLight(int row, int col, HighLight hightlight)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) gCallBack.Panic("Invalid cell index : " + row + ", " + col);
            Cells[row][col].HightLight = hightlight;
            Cells[row][col].Refresh();
        }

        public HighLight GetSquareHighLight (int square)
        {
            if (square < 0 || square > NumberSquares) gCallBack.Panic("Invalid square number: " + square);
            return Squares[square].HightLight;
        }

        public HighLight GetCellHighLight(int row, int col)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) gCallBack.Panic("Invalid cell index : " + row + ", " + col);
            return Cells[row][col].HightLight;
        }

        public int AddSwoop(int fromSquare, int toSquare)
        {
            if (fromSquare < 0 || fromSquare > NumberSquares) gCallBack.Panic("Invalid square number: " + fromSquare);
            if (toSquare < 0 || toSquare > NumberSquares) gCallBack.Panic("Invalid square number: " + toSquare);

            return AddSwoop (new Swoop() { From = SquareLocations[fromSquare], To = SquareLocations[toSquare] } );
        }

        public int AddSwoop(Swoop indicator)
        {
            var index = 0;
            lock(Swoops)
            {
                // get a new counter
                index = System.Threading.Interlocked.Increment(ref SwoopCount);

                // add a new swoop
                Swoops.Add(index, indicator);
            }

            // force a refresh
            IsDirty = true;
            Refresh();

            return index;
        }

        public bool GetSwoop(int index, out Swoop indicator)
        {
            lock(Swoops)
            {
                return Swoops.TryGetValue(index, out indicator);
            }
        }

        public bool RemoveSwoop(int index)
        {
            lock (Swoops)
            {
                if (!Swoops.ContainsKey(index)) return false;
                Swoops.Remove(index);
            }

            // forece a refresh
            IsDirty = true;
            Refresh();

            return true;
        }

        public void RemoveAllSwoops()
        {
            lock (Swoops)
            {
                Swoops.Clear();
            }
        }

#region private
        static Board()
        {
            // load the swoop images
            SwoopImages = new Dictionary<Direction, Bitmap>();
            SwoopImages.Add(Direction.Up_Pointing_Left, LoadImage(@"Media\swoop.png", Direction.Up_Pointing_Left, true));
            SwoopImages.Add(Direction.Up_Pointing_Right, LoadImage(@"Media\swoop.png", Direction.Up_Pointing_Right, true));
            SwoopImages.Add(Direction.Down_Pointing_Right, LoadImage(@"Media\swoop.png", Direction.Down_Pointing_Right, true));
            SwoopImages.Add(Direction.Down_Pointing_Left, LoadImage(@"Media\swoop.png", Direction.Down_Pointing_Left, true));

            SwoopCount = 0;
        }

        private static Bitmap LoadImage(string path, Direction direction, bool transparent = false)
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Assumption that the swoop starts points Up_Pointing_Left
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            var bitmap = new Bitmap(path);
            if (transparent)
            {
                bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            }
            // rotate
            switch (direction)
            {
                case Direction.Up_Pointing_Left: break;
                case Direction.Up_Pointing_Right: bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case Direction.Down_Pointing_Left: bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                case Direction.Down_Pointing_Right: bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                default:
                    gCallBack.Panic("Unknown direction : " + direction);
                    break;
            }
            return bitmap;
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;

            // add the swoop layer
            Overlay = new CodeProject.GraphicalOverlay();
            Overlay.Owner = this;
            Overlay.Paint += Overlay_Paint;

            // add grid
            Table = new TableLayoutPanel();
            Table.Width = Width;
            Table.Height = Height;
            Table.RowCount = BoardRowCount;
            Table.ColumnCount = BoardColumnCount;
            Table.Margin = new Padding(0);
            Table.Padding = new Padding(0);
            this.Controls.Add(Table);

            // add cells
            Cells = new Cell[Table.RowCount][];
            Squares = new Cell[NumberSquares + 1];
            Squares[0] = null;
            SquareLocations = new Location[NumberSquares + 1];
            SquareLocations[0] = new Location() { Row = 0, Col = 0 };
            
            int squareCounter = 1;
            for(int row = 0; row < Table.RowCount; row++)
            {
                Cells[row] = new Cell[Table.ColumnCount];
                for(int col=0; col< Table.ColumnCount; col++)
                {
                    int square = 0;
                    var state = CellState.Inactive;

                    if ((row % 2 != 0 && col % 2 == 0) ||
                        (row % 2 == 0 && col % 2 != 0))
                    {
                        square = squareCounter++;
                        if (WhiteOnTop) square = NumberSquares + 1 - square;
                        state = CellState.Empty;
                    }

                    Cell cell = new Cell(
                        state, 
                        square,
                        CellHeight,
                        CellWidth,
                        HighLight.None);
                    cell.MouseClick += Cell_MouseClick;
                    cell.MouseDoubleClick += Cell_MouseDoubleClick;
                    Cells[row][col] = cell;
                    if (square != 0)
                    {
                        Squares[square] = cell;
                        SquareLocations[square] = new Location() { Row = row, Col = col };
                    }

                    Table.Controls.Add(cell, col, row);
                }
            }
        }

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
//System.Diagnostics.Debug.WriteLine("Overlay_Paint " + IsDirty.ToString());
            // add the overlay swoops as necessary
            if (IsDirty || DoubleBuffer == null)
            {
                DoubleBuffer = new Bitmap(Height, Width);
                IsDirty = false;

                using (var g = Graphics.FromImage(DoubleBuffer))
                {
                    lock(Swoops)
                    {
                        // foreach swoop indicator add a swoop to the screen
                        foreach (var indicator in Swoops.Values)
                        {
                            // determine where to place the swoop
                            var direction = Direction.Up;
                            var x = 0;
                            var y = 0;
                            var width = 0;
                            var height = 0;

                            // up or down
                            if (indicator.From.Row < indicator.To.Row) direction = Direction.Down;
                            else direction = Direction.Up;

                            // left or right
                            if (indicator.From.Col < indicator.To.Col) direction |= Direction.Right;
                            else direction |= Direction.Left;

                            // starting where
                            x = Math.Min(indicator.From.Col, indicator.To.Col) * CellWidth;
                            y = Math.Min(indicator.From.Row, indicator.To.Row) * CellHeight;

                            // the width
                            width = (Math.Abs(indicator.From.Col - indicator.To.Col) + 1) * CellWidth;
                            height = (Math.Abs(indicator.From.Row - indicator.To.Row) + 1) * CellHeight;

                            var bitmap = SwoopImages[direction];
                            g.DrawImage(bitmap, x, y, width, height);
                        }
                    }
                }
            }

            e.Graphics.DrawImage(DoubleBuffer, 0, 0, Width, Height);
        }

        private void Cell_MouseClick(object sender, MouseEventArgs e)
        {
            var cell = sender as Cell;
            var square = cell.Square;
            if (square != 0) gCallBack.MouseClick(square);

            if (OnSelected != null)
            {
                var row = Table.GetRow(cell);
                var col = Table.GetColumn(cell);
                OnSelected(row, col, square);
            }
        }

        private void Cell_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var cell = sender as Cell;
            var square = cell.Square;
            if (square != 0) gCallBack.MouseDoubleClick(square);
        }

        enum Direction { Up = 1, Down = 2, Left = 4, Right = 8, Up_Pointing_Left = 5, Up_Pointing_Right = 9, Down_Pointing_Left = 6, Down_Pointing_Right = 10 };

        private TableLayoutPanel Table;
        private Cell[][] Cells;
        private Cell[] Squares;
        private Location[] SquareLocations;
        private int CellWidth;
        private int CellHeight;
        private GraphicalOverlay Overlay;
        private bool IsDirty;
        private Bitmap DoubleBuffer;
        private static Dictionary<Direction, Bitmap> SwoopImages;
        private Dictionary<int, Swoop> Swoops;
        private static int SwoopCount;

#endregion
    }
}
