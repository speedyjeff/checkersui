using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersUi
{
    partial class Moves
    {
        public Moves(int height, int width)
        {
            Height = height;
            Width = width;
            InitializeComponent();
        }

        public void AddRow(string red, string black)
        {
            var row = Table.NewRow();
            row["Red"] = red;
            row["Black"] = black;
            Table.Rows.Add(row);
        }

        public void UpdateRow(int index, string red, string black)
        {
            if (index < 0 || index >= Count) throw new Exception("Invalid row index : " + index);
            Table.Rows[index]["Red"] = red;
            Table.Rows[index]["Black"] = black;
        }

        public void GetRow(int index, out string red, out string black)
        {
            if (index < 0 || index >= Count) throw new Exception("Invalid row index : " + index);
            red = (string)Table.Rows[index]["Red"];
            black = (string)Table.Rows[index]["Black"];
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
            DataColumn red = new DataColumn("Red", typeof(string));
            DataColumn black = new DataColumn("Black", typeof(string));
            Table.Columns.Add(red);
            Table.Columns.Add(black);
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

