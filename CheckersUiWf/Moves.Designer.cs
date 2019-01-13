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

        public void AddRow(string white, string black)
        {
            var row = Table.NewRow();
            row[Word4White] = white;
            row[Word4Black] = black;
            Table.Rows.Add(row);
        }

        public void UpdateRow(int index, string white, string black)
        {
            if (index < 0 || index >= Count) CheckersCallBack.Panic("Invalid row index : " + index);
            Table.Rows[index][Word4White] = white;
            Table.Rows[index][Word4Black] = black;
        }

        public void GetRow(int index, out string white, out string black)
        {
            if (index < 0 || index >= Count) CheckersCallBack.Panic("Invalid row index : " + index);
            white = (string)Table.Rows[index][Word4White];
            black = (string)Table.Rows[index][Word4Black];
        }

        public int Count { get { return Table.Rows.Count; } }

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

            // create a DataTable
            Table = new DataTable("Moves");

            // Create two columns and add table to dataset
            DataColumn white = new DataColumn(Word4White, typeof(string));
            DataColumn black = new DataColumn(Word4Black, typeof(string));
            Table.Columns.Add(black); //black moves first and goes in the left column (first add)
            Table.Columns.Add(white);
            data.Tables.Add(Table);

            var grid = new DataGrid();
            grid.Size = new Size(Width, Height);
            grid.SetDataBinding(data, "Moves");

            Controls.Add(grid);
        }

        private DataTable Table;
        #endregion
    }


}

