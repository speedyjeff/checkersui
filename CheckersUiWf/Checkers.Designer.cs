using System;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;
using static CheckersUiWf.IntInterface;

namespace CheckersUiWf
{
    partial class CheckersFrm
    {
        internal void UpdateLeftStatus(String text)
        {
            StatusStripL.Items[0].Text = text;
        }

        internal void UpdateRightStatus(String text)
        {
            StatusStripR.Items[0].Text = text;
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
            Checkers = this;

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(Config.ClientSizeWidth, Config.ClientSizeHeight);
            this.Text = Config.AppName;
            this.DoubleBuffered = true;
            this.KeyPreview = true; //so that OnKeyPress works

            //
            //Application area
            //
            var panel = new TableLayoutPanel();
            panel.RowCount = 2;
            panel.ColumnCount = 2;
            panel.Dock = DockStyle.Fill;

            // the board
            Board = new BoardUct(Config.BoardWidth, Config.BoardHeight);
            Board.OnSelected += Board_OnSelected;
            panel.Controls.Add(Board, 1, 0);

            // the previous moves section
            Moves = new MovesUct(Config.MovesHeight, Config.MovesWidth);
            panel.Controls.Add(Moves, 0, 0);

            Controls.Add(panel);

            //
            // Status Line area
            //
            var panelS = new TableLayoutPanel();
            panelS.Height = Config.StatusHeight;
            panelS.RowCount = 1;
            panelS.ColumnCount = 2;
            panelS.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, (int)(Config.ClientSizeWidth * .4)));
            panelS.Dock = DockStyle.Bottom;

            // Left statusStrip
            StatusStripL = new StatusStrip();
            StatusStripL.Items.Add(new ToolStripStatusLabel());

            // Right statusStrip
            StatusStripR = new StatusStrip();
            StatusStripR.Items.Add(new ToolStripStatusLabel());

            panelS.Controls.Add(StatusStripL, 0, 0);
            panelS.Controls.Add(StatusStripR, 1, 0);

            Controls.Add(panelS);

            //
            // Menu area
            //
            var menuStrip1 = new MenuStrip();
            menuStrip1.AutoSize = false;
            menuStrip1.Height = Config.MenuHeight;

            // user initialization and menu items
            IntInterface.CallBack.InitializeCallout(menuStrip1);

            var eventHandler = new ToolStripItemClickedEventHandler(ToolStripMenuItem_DropDownItemClicked);
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.DropDownItemClicked += eventHandler;
            }
            Controls.Add(menuStrip1);
        }

        protected void ToolStripMenuItem_DropDownItemClicked(
            object sender, ToolStripItemClickedEventArgs e)
        {
            IntInterface.CallBack.ToolStripMenuItem_DropDownItemClicked(sender, e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            IntInterface.CallBack.OnKeyPress(e.KeyChar);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool rc = false;
            MoveDirection direction = Moves.CmdKey2Direction(keyData);
            if (direction == MoveDirection.OtherKey)
            {
                rc = IntInterface.CallBack.OnCmdKey(keyData);
                if (rc == false) rc = base.ProcessCmdKey(ref msg, keyData);
            }
            else //not a move table navigation cmd key
            {
                Moves.NavigateMovesByKey(direction);
                rc = true;
            }
            return rc;
        }

        //example of delegate event handler
        private void Board_OnSelected(int row, int col, int square)
        {
            IntInterface.CallBack.BoardClick(row, col, square);
        }

    }

#endregion
}

