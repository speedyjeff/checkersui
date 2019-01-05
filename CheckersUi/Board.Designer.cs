using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersUi
{
    public delegate void OnSelectedDelegate(int row, int col);

    partial class Board
    {
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            InitialPaint = true;
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

#region private
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
            Rand = new Random();
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
                        Table.Height / Table.RowCount,
                        Table.Width / Table.ColumnCount);
                    Cells[row][col].MouseClick += Cell_MouseClick;

                    Table.Controls.Add(Cells[row][col], col, row);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!InitialPaint) return;
            InitialPaint = false;

            for (int row = 0; row < Table.RowCount; row++)
            {
                for (int col = 0; col < Table.ColumnCount; col++)
                {
                    Cells[row][col].Refresh();
                }
            }
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

        private TableLayoutPanel Table;
        private Cell[][] Cells;
        private Random Rand;
        private bool InitialPaint;

#endregion
    }
}
