using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersUiWf
{
    public partial class CheckersFrm : Form
    {
        public CheckersFrm()
        {
            InitializeComponent();

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
            InitializeCallout(menuStrip1);

            var eventHandler = new ToolStripItemClickedEventHandler(ToolStripMenuItem_DropDownItemClicked);
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.DropDownItemClicked += eventHandler;
            }
            Controls.Add(menuStrip1);
        }

        public const string FileOpen = "FileOpen";
        public const string FileExit = "FileExit";

        internal void UpdateLeftStatus(String text)
        {
            StatusStripL.Items[0].Text = text;
        }

        internal void UpdateRightStatus(String text)
        {
            StatusStripR.Items[0].Text = text;
        }


        private BoardUct Board;
        private MovesUct Moves;
        private StatusStrip StatusStripR;
        private StatusStrip StatusStripL;

        private int Selected = 15;
        private int Target = 22;
        private int Swoop = Config.InvalidSwoop;

        private void InitializeCallout(MenuStrip menuStrip1)
        {
            //
            // File Menu
            //
            // openToolStripMenuItem
            var openToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            openToolStripMenuItem.Name = FileOpen;
            openToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.O)));
            openToolStripMenuItem.Text = "&Open";

            // toolStripSeparator1
            var toolStripSeparator1 = new ToolStripSeparator();
            toolStripSeparator1.Name = "toolStripSeparator1";

            // exitToolStripMenuItem
            var exitToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem.Name = FileExit;
            exitToolStripMenuItem.Text = "E&xit";

            // fileToolStripMenuItem
            var fileToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                openToolStripMenuItem,
                toolStripSeparator1,
                exitToolStripMenuItem});
            fileToolStripMenuItem.Text = "&File";

            menuStrip1.Items.Add(fileToolStripMenuItem);

            //
            // Game Menu
            //
            // newGameToolStripMenuItem
            var newGameToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            newGameToolStripMenuItem.Name = "New Game";
            newGameToolStripMenuItem.Text = "&New Game";

            // undoToolStripMenuItem
            var undoToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem.Name = "Undo";
            undoToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.Z)));
            undoToolStripMenuItem.Text = "&Undo";

            // gameToolStripMenuItem
            var gameToolStripMenuItem = new ToolStripMenuItem();
            gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                newGameToolStripMenuItem,
                undoToolStripMenuItem});
            gameToolStripMenuItem.Text = "&Game";

            // menuStrip1
            menuStrip1.Items.Add(gameToolStripMenuItem);

            // Set up example
            ExtInterface.NewGame();
            ExtInterface.SetSquareState(15, CellState.Black);
            ExtInterface.SetSquareState(11, CellState.Empty);
            ExtInterface.SetSquareState(18, CellState.White);
            ExtInterface.SetSquareState(22, CellState.Empty);
            ExtInterface.SetSquareHighLight(Selected, HighLight.Selected);
            ExtInterface.SetSquareHighLight(Target, HighLight.Target);
            Swoop = ExtInterface.AddSwoop(Selected, Target);
            MoveId move = new MoveId(Config.FirstMoveTableRow, CheckerColor.Black);
            ExtInterface.SetMoveText(move, "11-15");
            move.Color = CheckerColor.White;
            ExtInterface.SetMoveText(move, "22-18");
            move.Move++;
            move.Color = CheckerColor.Black;
            ExtInterface.SetMoveText(move, "(15x22)");
            ExtInterface.SetCurrentMove(MoveDirection.First);
            UpdateLeftStatus("Black to move");
            UpdateRightStatus("Black (12) White (12)");
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
            MoveDirection direction = CmdKey2Direction(keyData);
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

        // Move table navigation keys
        private MoveDirection CmdKey2Direction(Keys keyData)
        {
            MoveDirection direction = MoveDirection.OtherKey;
            if (keyData == Keys.Up)
                direction = MoveDirection.Up;
            else if (keyData == Keys.Down)
                direction = MoveDirection.Down;
            else if (keyData == Keys.Left)
                direction = MoveDirection.Left;
            else if (keyData == Keys.Right)
                direction = MoveDirection.Right;
            else if (keyData == Keys.B)
                direction = MoveDirection.RightPosition;
            else if (keyData == Keys.M)
                direction = MoveDirection.LeftPosition;
            else if (keyData == Keys.Home)
                direction = MoveDirection.First;
            else if (keyData == Keys.End)
                direction = MoveDirection.Last;
            return direction;
        }
    }
}
