using System.Windows.Forms;

namespace CheckersUi
{
    partial class Checkers
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 824);
            this.Text = "Checkers UI";
            this.DoubleBuffered = true;

            var panel = new TableLayoutPanel();
            panel.RowCount = 2;
            panel.ColumnCount = 2;
            panel.Dock = DockStyle.Fill;

            Controls.Add(panel);

            // the board
            Board = new Board(800, 800);
            Board.OnSelected += Board_OnSelected;
            Board.SetCellState(3, 1, CellState.Selected);
            panel.Controls.Add(Board, 1, 0);

            // the previous moves section
            Moves = new Moves(800, 200);
            panel.Controls.Add(Moves, 0, 0);
        }

        private void Board_OnSelected(int row, int col)
        {
            System.Diagnostics.Debug.WriteLine("Cell {0},{1} selected", row, col);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Board.Refresh();
        }

        private Board Board;
        private Moves Moves;

        #endregion
    }
}

