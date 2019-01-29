using System;
using System.Windows.Forms;
using static CheckersUiWf.Boundary;

namespace CheckersUiWf
{
    partial class Checkers
    {
        public void UpdateLeftStatus(String text)
        {
            statusStripL.Items[0].Text = text; 
        }

        public void UpdateRightStatus(String text)
        {
            statusStripR.Items[0].Text = text; 
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(ClientSizeWidth, ClientSizeHeight);
            this.Text = AppName;
            this.DoubleBuffered = true;
            this.KeyPreview = true; //so that OnKeyPress works
            gCallBack.CheckersObj = this;

            //
            //Application area
            //
            var panel = new TableLayoutPanel();
            panel.RowCount = 2;
            panel.ColumnCount = 2;
            panel.Dock = DockStyle.Fill;

            // the board
            Board = new Board(BoardWidth, BoardHeight);
            gCallBack.BoardObj = Board;
            Board.OnSelected += Board_OnSelected;
            panel.Controls.Add(Board, 1, 0);

            // the previous moves section
            Moves = new Moves(MovesHeight, MovesWidth);
            gCallBack.MovesObj = Moves;
            panel.Controls.Add(Moves, 0, 0);

            Controls.Add(panel);

            //
            //Status Line area
            //
            var panelS = new TableLayoutPanel();
            panelS.Height = StatusHeight;
            panelS.RowCount = 1;
            panelS.ColumnCount = 2;
            panelS.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, (int)(ClientSizeWidth * .4)));
            panelS.Dock = DockStyle.Bottom;

            // Left statusStrip
            statusStripL = new StatusStrip();
            statusStripL.Items.Add(new ToolStripStatusLabel());

            // Right statusStrip
            statusStripR = new StatusStrip();
            statusStripR.Items.Add(new ToolStripStatusLabel());

            panelS.Controls.Add(statusStripL,0,0);
            panelS.Controls.Add(statusStripR,1,0);
            Controls.Add(panelS);

            //
            //Menu area
            //
            var menuStrip1 = new MenuStrip();
            menuStrip1.AutoSize = false;
            menuStrip1.Height = MenuHeight;
            InitializeMenu(menuStrip1);
            Controls.Add(menuStrip1);

            // initialize call back interface 
            gCallBack.Initialize();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            char key = e.KeyChar;
            gCallBack.OnKeyPress(key);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool rc = gCallBack.ProcessCmdKey(keyData);
            if (rc == false) rc = base.ProcessCmdKey(ref msg, keyData);
            return rc;
        }

        //example of delegate event handler
        private void Board_OnSelected(int row, int col, int square)
        {
            gCallBack.BoardClick(row, col, square);
        }

        private Board Board;
        private Moves Moves;
        private StatusStrip statusStripR;
        private StatusStrip statusStripL;

#endregion
    }
}

