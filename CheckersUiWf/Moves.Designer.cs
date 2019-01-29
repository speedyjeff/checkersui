using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static CheckersUiWf.Boundary;

namespace CheckersUiWf
{
    partial class Moves
    {
        public Moves(int height, int width)
        {
            Height = height;
            Width = width;
            InitializeComponent();
        }

        public void AddRow(string black, string white)
        {
            string[] row = { (Count+1).ToString() + ".", black, white };
            Grid.Rows.Add(row);
        }

        public void SetMoveText(MoveId moveId, string value)
        {
            if (moveId.move < 0 ) gCallBack.Panic("Invalid row index : " + moveId.move);
            while (moveId.move > Count)
            {
                AddRow("", "");
            }
            Grid[moveId.color, moveId.move - 1].Value = value;
        }

        public string GetMoveText(MoveId moveId)
        {
            string text = "";
            if (moveId.move < 1) moveId.move = 1;
            else if (moveId.move > Count) moveId.move = Count;
            text = (string)Grid[moveId.color, moveId.move -1].Value;
            return text;
        }

        public void SetCurrentMove(MoveId moveId)
        {
            if (moveId.move < 1) moveId.move = 1;
            else if (moveId.move > Count) moveId.move = Count;
            Grid.CurrentCell = Grid[moveId.color, moveId.move - 1];
        }

        public MoveId GetCurrentMove()
        {
            MoveId moveId = new MoveId();
            if (Count > 1)
            {
                moveId.move = Grid.CurrentCell.RowIndex + 1;
                int col = Grid.CurrentCell.ColumnIndex;
                moveId.color = Grid.Columns[col].Name;
            }
            return moveId;
        }

        public int Count { get { return Grid.Rows.Count; } }

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

            var data = new DataSet("Previous");

            Grid = new MyDataGridView();
            Grid.Name = "Moves"; 
            Grid.Size = new Size(Width, Height);
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.ColumnCount = 3;
            int w = (MovesWidth - MoveColumnWidth - 42/*width of left strip*/) / 2;
            Grid.Columns[0].Name = MoveColumn;
            Grid.Columns[0].ReadOnly = true;
            Grid.Columns[0].Width = MoveColumnWidth;
            Grid.Columns[1].Name = BlackColumn;
            Grid.Columns[1].Width = w;
            Grid.Columns[1].ReadOnly = true;
            Grid.Columns[2].Name = WhiteColumn;
            Grid.Columns[2].Width = w;
            Grid.Columns[2].ReadOnly = true;

            Controls.Add(Grid);
        }

        private class MyDataGridView : DataGridView
        {
            public MyDataGridView() { }

            protected override void OnCellClick(DataGridViewCellEventArgs e)
            {
                bool rc = false;
                int row = e.RowIndex;
                int col = e.ColumnIndex;
                if (col > 0 && row >= 0)
                {
                    rc = gCallBack.MoveClick(new MoveId(row + 1, this.Columns[col].Name));
                }
                if (!rc) base.OnCellClick(e);
            }
        }

        private DataGridView Grid;
    }
    #endregion
}
