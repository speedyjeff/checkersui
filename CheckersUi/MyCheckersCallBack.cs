using System;

using CheckersUiWf;

namespace CheckersUi
{
    public class MyCheckersCallBack: CallBack
    {
        public override void Trace(string text)
        {
            System.Diagnostics.Debug.WriteLine("MyCallBack: " + text);
        }

        public override bool MenuItemSelect(string menuName)
        {
            bool handled = false;
            switch (menuName)
            {
                case CheckersFrm.FileOpen:
                    OpenFile();
                    handled = true;
                    break;
                case CheckersFrm.FileExit:
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
                if (Target != Config.InvalidSquare)
                {
                    SetSquareHighLight(Target, HighLight.None);
                    if (Swoop != Config.InvalidSwoop)
                    {
                        RemoveSwoop(Swoop);
                        Swoop = Config.InvalidSwoop;
                    }
                }
                if (Target == square)
                {
                    Target = Config.InvalidSquare;
                }
                else
                {
                    SetSquareHighLight(square, HighLight.Target);
                    Target = square;
                }
            }
            else if (state != CellState.Inactive) //a checker or king
            {
                if (Selected != Config.InvalidSquare)
                {
                    SetSquareHighLight(Selected, HighLight.None);
                    if (Swoop != Config.InvalidSwoop)
                    {
                        RemoveSwoop(Swoop);
                        Swoop = Config.InvalidSwoop;
                    }
                }
                if (Selected == square)
                {
                    Selected = Config.InvalidSquare;
                }
                else
                {
                    SetSquareHighLight(square, HighLight.Selected);
                    Selected = square;
                }
            }
            if (Target != Config.InvalidSquare && Selected != Config.InvalidSquare && Swoop == Config.InvalidSwoop)
            {
                Swoop = AddSwoop(Selected, Target);
            }
            else if (Swoop != InvalidSwoop)
            {
                RemoveSwoop(Swoop);
                Swoop = Config.InvalidSwoop;
            }
        }

        public override void OnKeyPress(char key)
        {
            Trace("OnKeyPress " + key);

            switch (key)
            {
                case 'x':
                case 'X':
                    if (Selected != Config.InvalidSquare)
                    {
                        SetSquare(Selected, CellState.Empty, HighLight.None);
                        Selected = Config.InvalidSquare;
                        if (Swoop != Config.InvalidSwoop)
                        {
                            RemoveSwoop(Swoop);
                            Swoop = Config.InvalidSwoop;
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
