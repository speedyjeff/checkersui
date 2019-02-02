using CodeProject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;
using static CheckersUiWf.IntInterface;

namespace CheckersUiWf
{
    public delegate void OnClickDelegate(int row, int col, int square);

    partial class BoardUct
    {
        internal BoardUct(int width, int height)
        {
            Width = width;
            Height = height;
            IsDirty = false;
            Swoops = new Dictionary<int, Swoop>();
            CellWidth = width / BoardColumnCount;
            CellHeight = height / BoardRowCount;
            InitializeComponent();
        }

        internal event OnClickDelegate OnSelected;

        internal void NewGame()
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

        internal void SetSquare(int square, CellState state, HighLight highlight)
        {
            if (square < 0 || square > NumberSquares) CallBack.Panic("Invalid square number: " + square);
            Squares[square].CellState = state;
            Squares[square].HightLight = highlight;
            Squares[square].Refresh();
        }

        internal void SetSquareState(int square, CellState state)
        {
            if (square < 0 || square > NumberSquares) CallBack.Panic("Invalid square number: " + square);
            Squares[square].CellState = state;
            Squares[square].Refresh();
        }

        internal void SetCellState(int row, int col, CellState state)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) CallBack.Panic("Invalid cell index : " + row + ", " + col);
            Cells[row][col].CellState = state;
            Cells[row][col].Refresh();
        }

        internal CellState GetSquareState(int square)
        {
            if (square < 0 || square > NumberSquares) CallBack.Panic("Invalid square number: " + square);
            return Squares[square].CellState;
        }

        internal CellState GetCellState(int row, int col)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) CallBack.Panic("Invalid cell index : " + row + ", " + col);
            return Cells[row][col].CellState;
        }

        internal void SetSquareHighLight (int square, HighLight highlight)
        {
            if (square < 0 || square > NumberSquares) CallBack.Panic("Invalid square number: " + square);
            Squares[square].HightLight = highlight;
            Squares[square].Refresh();
        }

        internal void SetCellHightLight(int row, int col, HighLight hightlight)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) CallBack.Panic("Invalid cell index : " + row + ", " + col);
            Cells[row][col].HightLight = hightlight;
            Cells[row][col].Refresh();
        }

        internal HighLight GetSquareHighLight (int square)
        {
            if (square < 0 || square > NumberSquares) CallBack.Panic("Invalid square number: " + square);
            return Squares[square].HightLight;
        }

        internal HighLight GetCellHighLight(int row, int col)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) CallBack.Panic("Invalid cell index : " + row + ", " + col);
            return Cells[row][col].HightLight;
        }

        internal int AddSwoop(int fromSquare, int toSquare)
        {
            if (fromSquare < 0 || fromSquare > NumberSquares) CallBack.Panic("Invalid square number: " + fromSquare);
            if (toSquare < 0 || toSquare > NumberSquares) CallBack.Panic("Invalid square number: " + toSquare);

            if (Config.ColorOnTop == CheckerColor.White)
            {
                fromSquare = NumberSquares - fromSquare + 1;
                toSquare = NumberSquares - toSquare + 1;
            }
            return AddSwoop (new Swoop() { From = SquareLocations[fromSquare], To = SquareLocations[toSquare] } );
        }

        internal int AddSwoop(Swoop indicator)
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

        internal bool GetSwoop(int index, out Swoop indicator)
        {
            lock(Swoops)
            {
                return Swoops.TryGetValue(index, out indicator);
            }
        }

        internal bool RemoveSwoop(int index)
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

        internal void RemoveAllSwoops()
        {
            lock (Swoops)
            {
                Swoops.Clear();
            }
            IsDirty = true;
            Refresh();
        }

        internal string[] CellNumbers
        {
            get
            {
                return Cell.CellNumbers;
            }
            set
            {
                Cell.CellNumbers = value;
                foreach (Cell cell in Squares)
                {
                    if (cell != null) cell.Dirty = true;
                }
                IsDirty = true;
                Refresh();
            }
        }

        internal bool SetColorOnTop(CheckerColor color)
        {
            bool change = false;

            int squareOnTop = Cells[0][1].Square;

            if ((squareOnTop == 1 && color == CheckerColor.White) ||   //black on top
                (squareOnTop != 1 && color == CheckerColor.Black))
            {
                change = true;
                Config.ColorOnTop = color;

                for (int square1 = 1; square1 <= (NumberSquares / 2); square1++)
                {
                    int square2 = NumberSquares - square1 + 1;

                    Cell cell1 = Squares[square1];
                    int number1 = Squares[square1].Square;
                    HighLight highLight1 = Squares[square1].HightLight;
                    CellState state1 = Squares[square1].CellState;

                    Cell cell2 = Squares[square2];
                    int number2 = Squares[square2].Square;
                    HighLight highLight2 = Squares[square2].HightLight;
                    CellState state2 = Squares[square2].CellState;

                    Squares[square1].Square = number2;
                    Squares[square1].HightLight = highLight2;
                    Squares[square1].CellState = state2;
                    Squares[square1].Refresh();

                    Squares[square2].Square = number1;
                    Squares[square2].HightLight = highLight1;
                    Squares[square2].CellState = state1;
                    Squares[square2].Refresh();

                    Squares[square1] = cell2;
                    Squares[square2] = cell1;
                }
                // flip the swoops
                lock (Swoops)
                {
                    List<int> swoopList = new List<int>();
                    foreach (int key in Swoops.Keys) swoopList.Add(key);
                    foreach (int key in swoopList)
                    {
                        var swoop = Swoops[key];
                        swoop.From.Row = BoardRowCount - swoop.From.Row - 1;
                        swoop.From.Col = BoardColumnCount - swoop.From.Col - 1;
                        swoop.To.Row = BoardRowCount - swoop.To.Row - 1;
                        swoop.To.Col = BoardColumnCount - swoop.To.Col - 1;
                        Swoops[key] = swoop;
                    }
                }
                IsDirty = true;
                Refresh();
            }

            return change;
        }

#region private
        static BoardUct()
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
                    CallBack.Panic("Unknown direction : " + direction);
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
                        if (Config.ColorOnTop == CheckerColor.White) square = NumberSquares + 1 - square;
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
            if (square != 0) CallBack.MouseClick(square);

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
            if (square != 0) CallBack.MouseDoubleClick(square);
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
