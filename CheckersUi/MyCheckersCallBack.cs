using System;
using System.IO;
using CheckersUiWf;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;

namespace CheckersUi
{
    public class MyCheckersCallBack: CallBack
    {
        public MyCheckersCallBack() { }

        public override void Trace(string text)
        {
            System.Diagnostics.Debug.WriteLine("MyCallBack: " + text);
        }

        public override void InitializeCallout(MenuStrip menuStrip1)
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
            NewGame();
            SetSquareState(15, CellState.Black);
            SetSquareState(11, CellState.Empty);
            SetSquareState(18, CellState.White);
            SetSquareState(22, CellState.Empty);
            SetSquareHighLight(Selected, HighLight.Selected);
            SetSquareHighLight(Target, HighLight.Target);
            Swoop = AddSwoop(Selected, Target);
            MoveId move = new MoveId(FirstMoveTableRow, CheckerColor.Black);
            SetMoveText(move, "11-15");
            move.Color = CheckerColor.White;
            SetMoveText(move, "22-18");
            move.Move++;
            move.Color = CheckerColor.Black;
            SetMoveText(move, "(15x22)");
            SetCurrentMove(MoveDirection.First);
            UpdateLeftStatus("Black to move");
            UpdateRightStatus("Black (12) White (12)");
        }

        public override bool ToolStripMenuItem_DropDownItemClicked(
            object sender, ToolStripItemClickedEventArgs e)
        {
            bool handled = false;
            switch (e.ClickedItem.Name)
            {
                case FileOpen:
                    OpenFile();
                    handled = true;
                    break;
                case FileExit:
                    Application.Exit();
                    handled = true;
                    break;
            }
            UpdateLeftStatus(String.Format("{0} selected", e.ClickedItem.Text));

            return handled;
        }

        public override void MouseClick(int square)
        {
            Trace("MouseClick " + square.ToString());

            CellState state = GetSquareState(square);

            if (state == CellState.Empty)
            {
                if (Target != InvalidSquare)
                {
                    SetSquareHighLight(Target, HighLight.None);
                    if (Swoop != InvalidSwoop)
                    {
                        RemoveSwoop(Swoop);
                        Swoop = InvalidSwoop;
                    }
                }
                if (Target == square)
                {
                    Target = InvalidSquare;
                }
                else
                {
                    SetSquareHighLight(square, HighLight.Target);
                    Target = square;
                }
            }
            else if (state != CellState.Inactive) //a checker or king
            {
                if (Selected != InvalidSquare)
                {
                    SetSquareHighLight(Selected, HighLight.None);
                    if (Swoop != InvalidSwoop)
                    {
                        RemoveSwoop(Swoop);
                        Swoop = InvalidSwoop;
                    }
                }
                if (Selected == square)
                {
                    Selected = InvalidSquare;
                }
                else
                {
                    SetSquareHighLight(square, HighLight.Selected);
                    Selected = square;
                }
            }
            if (Target != InvalidSquare && Selected != InvalidSquare && Swoop == InvalidSwoop)
            {
                Swoop = AddSwoop(Selected, Target);
            }
            else if (Swoop != InvalidSwoop)
            {
                RemoveSwoop(Swoop);
                Swoop = InvalidSwoop;
            }
        }

        public override void OnKeyPress(char key)
        {
            Trace("OnKeyPress " + key);

            switch (key)
            {
                case 'x':
                case 'X':
                    if (Selected != InvalidSquare)
                    {
                        SetSquare(Selected, CellState.Empty, HighLight.None);
                        Selected = InvalidSquare;
                        if (Swoop != InvalidSwoop)
                        {
                            RemoveSwoop(Swoop);
                            Swoop = InvalidSwoop;
                        }
                    }
                    break;
                case 'n':
                case 'N':
                    UpdateCellNumbers(RowColumn);
                    break;
                case 'b':
                case 'B':
                    OutPut("************************************");
                    break;
            }
        }

        public override void MouseDoubleClick(int square)
        {
            Trace("MouseDoubleClick Square " + square.ToString());

            switch (GetSquareState(square))
            {
                case CellState.White:
                    SetSquareState(square, CellState.WhiteKing);
                    break;
                case CellState.WhiteKing:
                    SetSquareState(square, CellState.White);
                    break;
                case CellState.Black:
                    SetSquareState(square, CellState.BlackKing);
                    break;
                case CellState.BlackKing:
                    SetSquareState(square, CellState.Black);
                    break;
            }
        }

        #region private
        private void OpenFile()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private const string FileOpen = "FileOpen";
        private const string FileExit = "FileExit";
        private int Selected = 15;
        private int Target = 22;
        private int Swoop = InvalidSwoop;
        
        private static string[] RowColumn =
        {
            "",
            "B8","D8","F8","H8","A7","C7","E7","G7",
            "B6","D6","F6","H6","A5","C5","E5","G5",
            "B4","D4","F4","H4","A3","C3","E3","G3",
            "B2","D2","F2","H2","A1","C1","E1","G1",
        }; 
        #endregion
    }
}
