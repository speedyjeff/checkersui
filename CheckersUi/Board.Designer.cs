using CodeProject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersUi
{
    public delegate void OnSelectedDelegate(int row, int col);
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
            CellWidth = width / 8;
            CellHeight = height / 8;
            InitializeComponent();
        }

        public event OnSelectedDelegate OnSelected;

        public void SetCellState(int row, int col, CellState state)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) throw new Exception("Invalid cell index : " + row + ", " + col);
            Cells[row][col].CellState = state;
            Cells[row][col].Refresh();
        }

        public CellState GetCellState(int row, int col)
        {
            if (row < 0 || row > Cells.Length ||
                col < 0 || col > Cells[row].Length) throw new Exception("Invalid cell index : " + row + ", " + col);
            return Cells[row][col].CellState;
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
            Refresh();

            return true;
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
                default: throw new Exception("Unknown direction : " + direction);
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

            // add grib
            Table = new TableLayoutPanel();
            Table.Width = Width;
            Table.Height = Height;
            Table.RowCount = 8;
            Table.ColumnCount = 8;
            Table.Margin = new Padding(0);
            Table.Padding = new Padding(0);

            this.Controls.Add(Table);

            // add cells
            Cells = new Cell[Table.RowCount][];
            for(int row = 0; row < Table.RowCount; row++)
            {
                Cells[row] = new Cell[Table.ColumnCount];
                for(int col=0; col< Table.ColumnCount; col++)
                {

                    var state = CellState.Inative;

                    if ((row % 2 == 0 && col % 2 == 0)
                        ||
                        (row % 2 != 0 && col % 2 != 0))
                    {
                        if (row <= 2) state = CellState.Black;
                        else if (row >= 5) state = CellState.Red;
                        else state = CellState.Active;
                    }

                    Cells[row][col] = new Cell(
                        state, 
                        (row * Table.RowCount) + col,
                        CellHeight,
                        CellWidth);
                    Cells[row][col].MouseClick += Cell_MouseClick;

                    Table.Controls.Add(Cells[row][col], col, row);
                }
            }
        }

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
            // add the overlay swoops as necessary
            if (IsDirty || DoubleBuffer == null)
            {
                DoubleBuffer = new Bitmap(Height, Width);

                using (var g = Graphics.FromImage(DoubleBuffer))
                {
                    lock(Swoops)
                    {
                        // foreach swoop indicator add a swoop to the screen
                        foreach(var indicator in Swoops.Values)
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
            var row = Table.GetRow(cell);
            var col = Table.GetColumn(cell);

            if (OnSelected != null)
            {
                OnSelected(row, col);
            }
        }

        enum Direction { Up = 1, Down = 2, Left = 4, Right = 8, Up_Pointing_Left = 5, Up_Pointing_Right = 9, Down_Pointing_Left = 6, Down_Pointing_Right = 10 };

        private TableLayoutPanel Table;
        private Cell[][] Cells;
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
