using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace CheckersUiWf
{
    partial class MovesUct : UserControl
    {
        public MovesUct(int height, int width)
        {
            Height = height;
            Width = width;
            InitializeComponent();

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

        #region private
        private DataGridView Grid;
        internal MoveId LastCurrentMove = new MoveId();

        internal int Count { get { return Grid.Rows.Count; } }

        internal void ClearMoves()
        {
            while (Grid.Rows.Count > 0) Grid.Rows.RemoveAt(0);
        }

        internal void AddRow(string black, string white)
        {
            string[] row = { (Count + 1).ToString() + ".", black, white };
            Grid.Rows.Add(row);
        }

        internal void SetMoveText(MoveId moveId, string value)
        {
            if (moveId.Move < 1) IntInterface.CallBack.Panic("Invalid row index : " + moveId.Move);
            while (moveId.Move > Count)
            {
                AddRow(Config.BlankTableEntry, Config.BlankTableEntry);
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
                IntInterface.CallBack.Panic("SetCurrentMove invalid moveId=" + moveId.ToString());
            }
        }

        internal void SetCurrentMove(MoveDirection direction)
        {
            if (direction != MoveDirection.OtherKey)
            {
                NavigateMovesByKey(direction);
            }
            else
            {
                IntInterface.CallBack.Panic("SetCurrentMove invalid direction=" + direction.ToString());
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

        internal void NavigateMovesByKey(MoveDirection moveDirection)
        {
            bool newCurrent = false;
            MoveId moveId = null;
            do // error exit
            {
                if (Count < 1) break; //No table to navigate

                int move = Config.InvalidMove;
                CheckerColor color = CheckerColor.Invalid;
                if (moveDirection == MoveDirection.First)
                {
                    move = Config.FirstMoveTableRow;
                    color = CheckerColor.Black;
                }
                else if (moveDirection == MoveDirection.Last)
                {
                    move = Count;
                    color = CheckerColor.White;
                }
                if (move != Config.InvalidMove)
                {
                    moveId = new MoveId(move, color);
                    if (GetMoveText(moveId) != Config.BlankTableEntry)
                    {
                        newCurrent = true;
                        break;
                    }
                    moveId.Color = (color == CheckerColor.Black ? CheckerColor.White : CheckerColor.Black);
                    if (GetMoveText(moveId) != Config.BlankTableEntry)
                    {
                        newCurrent = true;
                        break;
                    }
                }

                moveId = GetCurrentMove();
                if (!moveId.IsValid) IntInterface.CallBack.Panic("NavigateMoves invalid moveId=" + moveId.ToString());

                switch (moveDirection)
                {
                    case MoveDirection.Down:
                        {
                            if (moveId.Move == Count) break;
                            moveId.Move++;
                            if (GetMoveText(moveId) == Config.BlankTableEntry)
                            {
                                moveId.Move--;
                                break;
                            }
                            newCurrent = true;
                            break;
                        }
                    case MoveDirection.Up:
                        {
                            if (moveId.Move == 1) break;
                            moveId.Move--;
                            if (GetMoveText(moveId) == Config.BlankTableEntry)
                            {
                                moveId.Move++;
                                break;
                            }
                            newCurrent = true;
                            break;
                        }
                    case MoveDirection.LeftPosition:
                        {
                            if (moveId.Position == MoveState.After)
                            {
                                moveId.Position = MoveState.Before;
                            }
                            else if (moveId.Color == CheckerColor.Black)
                            {
                                if (moveId.Move == 1) break;
                                moveId.Move--;
                                moveId.Color = CheckerColor.White;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Move++;
                                    moveId.Color = CheckerColor.Black;
                                    break;
                                }
                                moveId.Position = MoveState.After;
                            }
                            else if (moveId.Color == CheckerColor.White)
                            {
                                moveId.Color = CheckerColor.Black;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Color = CheckerColor.White;
                                    break;
                                }
                                moveId.Position = MoveState.After;
                            }
                            newCurrent = true;
                            break;
                        }
                    case MoveDirection.Left:
                        {
                            if (moveId.Color == CheckerColor.Black)
                            {
                                if (moveId.Move == 1) break;
                                moveId.Move--;
                                moveId.Color = CheckerColor.White;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Move++;
                                    moveId.Color = CheckerColor.Black;
                                    break;
                                }
                                moveId.Position = MoveState.Before;
                            }
                            else if (moveId.Color == CheckerColor.White)
                            {
                                moveId.Color = CheckerColor.Black;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Color = CheckerColor.White;
                                    break;
                                }
                                moveId.Position = MoveState.Before;
                            }
                            newCurrent = true;
                            break;
                        }
                    case MoveDirection.RightPosition:
                        {
                            if (moveId.Position == MoveState.Before)
                            {
                                moveId.Position = MoveState.After;
                            }
                            else if (moveId.Color == CheckerColor.White)
                            {
                                if (moveId.Move == Count) break;
                                moveId.Move++;
                                moveId.Color = CheckerColor.Black;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Move--;
                                    moveId.Color = CheckerColor.White;
                                    break;
                                }
                                moveId.Position = MoveState.Before;
                            }
                            else if (moveId.Color == CheckerColor.Black)
                            {
                                moveId.Color = CheckerColor.White;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Color = CheckerColor.Black;
                                    break;
                                }
                                moveId.Position = MoveState.Before;
                            }
                            newCurrent = true;
                            break;
                        }
                    case MoveDirection.Right:
                        {
                            if (moveId.Color == CheckerColor.White)
                            {
                                if (moveId.Move == Count) break;
                                moveId.Move++;
                                moveId.Color = CheckerColor.Black;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Move--;
                                    moveId.Color = CheckerColor.White;
                                    break;
                                }
                                moveId.Position = MoveState.Before;
                            }
                            else if (moveId.Color == CheckerColor.Black)
                            {
                                moveId.Color = CheckerColor.White;
                                if (GetMoveText(moveId) == Config.BlankTableEntry)
                                {
                                    moveId.Color = CheckerColor.Black;
                                    break;
                                }
                                moveId.Position = MoveState.Before;
                            }
                            newCurrent = true;
                            break;
                        }
                }
            } while (false);

            if (newCurrent && moveId != null)
            {
                SetCurrentMove(moveId);
                IntInterface.CallBack.MoveSelect(moveId);
            }
        }

        internal void NavigateMovesByMouse(MoveId moveId)
        {
            if (!moveId.IsValid)
            {
                SetCurrentMove(LastCurrentMove);
            }
            else
            {
                if (GetMoveText(moveId) == Config.BlankTableEntry)
                {
                    SetCurrentMove(LastCurrentMove);
                }
                else
                {
                    SetCurrentMove(moveId);
                    IntInterface.CallBack.MoveSelect(moveId);
                }
            }
        }
        #endregion
    }
}
