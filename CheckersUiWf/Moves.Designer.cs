using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;
using static CheckersUiWf.IntInterface;

namespace CheckersUiWf
{
    partial class MovesUct
    {
        internal int Count { get { return Grid.Rows.Count; } }

        internal MovesUct(int height, int width)
        {
            Height = height;
            Width = width;
            InitializeComponent();
        }

        internal void AddRow(string black, string white)
        {
            string[] row = { (Count+1).ToString() + ".", black, white };
            Grid.Rows.Add(row);
        }

        internal void SetMoveText(MoveId moveId, string value)
        {
            if (moveId.Move < 1 ) CallBack.Panic("Invalid row index : " + moveId.Move);
            while (moveId.Move > Count)
            {
                AddRow(BlankTableEntry, BlankTableEntry);
            }
            Grid[moveId.ColumnName, moveId.Row].Value = value;
        }

        internal string GetMoveText(MoveId moveId)
        {
            string text = "";
            if (moveId.IsValid)
                text = (string)Grid[moveId.ColumnName, moveId.Row].Value;
            return text;
        }

        internal void SetCurrentMove(MoveId moveId)
        {
            if (moveId.IsValid)
            {
                Grid.CurrentCell = Grid[moveId.ColumnName, moveId.Row];
                LastCurrentMove = moveId.ShallowCopy();
            }
            else
            {
                CallBack.Panic("SetCurrentMove invalid moveId=" + moveId.ToString());
            }
        }

        internal MoveId GetCurrentMove()
        {
            MoveId moveId = new MoveId(Grid.CurrentCell.RowIndex, Grid.CurrentCell.ColumnIndex);
            if ((moveId.Move == LastCurrentMove.Move) &&
                (moveId.Color == LastCurrentMove.Color))
            {
                moveId.Position = LastCurrentMove.Position;
            }
            return moveId;
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

            var data = new DataSet("Previous");

            Grid = new MyDataGridView();
            Grid.Name = "Moves"; 
            Grid.Size = new Size(Width, Height);
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.ColumnCount = 3;
            int w = (Config.MovesWidth - Config.MoveColumnWidth - 42/*width of left strip*/) / 2;
            Grid.Columns[0].Name = Config.MoveColumn;
            Grid.Columns[0].ReadOnly = true;
            Grid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns[0].Width = Config.MoveColumnWidth;
            Grid.Columns[1].Name = Config.BlackColumn;
            Grid.Columns[1].Width = w;
            Grid.Columns[1].ReadOnly = true;
            Grid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns[2].Name = Config.WhiteColumn;
            Grid.Columns[2].Width = w;
            Grid.Columns[2].ReadOnly = true;
            Grid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

            Controls.Add(Grid);
        }

        private class MyDataGridView : DataGridView
        {
            public MyDataGridView() { }

            protected override void OnCellClick(DataGridViewCellEventArgs e)
            {
                Moves.NavigateMovesByMouse(new MoveId(e.RowIndex, e.ColumnIndex));
            }
        }

        internal MoveId LastCurrentMove = new MoveId();
    }
#endregion
}
