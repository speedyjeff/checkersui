using System;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;
using static CheckersUiWf.IntInterface;

namespace CheckersUiWf
{
    partial class MoveId // used internally for layout grid
    {
        internal MoveId(int row, string columnName, MoveState position = MoveState.Before)
        {
            Move = row + 1;
            Color = Column2Color(columnName);
            Position = position;
        }
        internal MoveId(int row, int column, MoveState position = MoveState.Before)
        {
            Move = row + 1;
            Color = Column2Color(column > 0 ? Grid.Columns[column].Name : Config.MoveColumn );
            Position = position;
        }
        internal String ColumnName { get => Color2ColumnName(color); set => color = Column2Color(value); }
        internal int Row { get => move - 1; set => move = value + 1; }
        internal bool IsValid { get =>
                    (move > 0 &&
                     color != CheckerColor.Invalid &&
                     move <= Moves.Count); }

        private static string Color2ColumnName(CheckerColor color)
        {
            if (color == CheckerColor.Black) return Config.BlackColumn;
            if (color == CheckerColor.White) return Config.WhiteColumn;
            return Config.MoveColumn;
        }

        private static CheckerColor Column2Color(string column)
        {
            if (column == Config.BlackColumn) return CheckerColor.Black;
            if (column == Config.WhiteColumn) return CheckerColor.White;
            return CheckerColor.Invalid;
        }
    }

    partial class MovesUct
    {
        internal void NavigateMovesByKey(MoveDirection moveDirection)
        {
            bool newCurrent = false;
            MoveId moveId = null;

            do // error exit
            {
                if (Moves.Count < 1) break; //No table to navigate

                int move = InvalidMove;
                CheckerColor color = CheckerColor.Invalid;
                if (moveDirection == MoveDirection.First)
                {
                    move = FirstMoveTableRow;
                    color = CheckerColor.Black;
                }
                else if (moveDirection == MoveDirection.Last)
                {
                    move = Moves.Count;
                    color = CheckerColor.White;
                }
                if (move != InvalidMove)
                {
                    moveId = new MoveId(move, color);
                    if (GetMoveText(moveId) != BlankTableEntry)
                    {
                        newCurrent = true;
                        break;
                    }
                    moveId.Color = (color == CheckerColor.Black ? CheckerColor.White : CheckerColor.Black);
                    if (GetMoveText(moveId) != BlankTableEntry)
                    {
                        newCurrent = true;
                        break;
                    }
                }

                moveId = Moves.GetCurrentMove();
                if (!moveId.IsValid) IntInterface.CallBack.Panic("NavigateMoves invalid moveId=" + moveId.ToString());

                switch (moveDirection)
                {
                    case MoveDirection.Down:
                        {
                            if (moveId.Move == Count) break;
                            moveId.Move++;
                            if (GetMoveText(moveId) == BlankTableEntry)
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
                            if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
                                if (GetMoveText(moveId) == BlankTableEntry)
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
            } while (false) ;

            if (newCurrent && moveId != null)
            {
                Moves.SetCurrentMove(moveId);
                IntInterface.CallBack.MoveSelect(moveId);
            }
        }

        internal MoveDirection CmdKey2Direction (Keys keyData)
        {
            MoveDirection direction = MoveDirection.OtherKey;
            if (keyData == Config.MoveUp)
                direction = MoveDirection.Up;
            else if (keyData == Config.MoveDown)
                direction = MoveDirection.Down;
            else if (keyData == Config.MoveLeft)
                direction = MoveDirection.Left;
            else if (keyData == Config.MoveRight)
                direction = MoveDirection.Right;
            else if (keyData == Config.MoveRightPosition)
                direction = MoveDirection.RightPosition;
            else if (keyData == Config.MoveLeftPosition)
                direction = MoveDirection.LeftPosition;
            else if (keyData == Config.MoveFirst)
                direction = MoveDirection.First;
            else if (keyData == Config.MoveLast)
                direction = MoveDirection.Last;
            return direction;
        }

        internal void NavigateMovesByMouse(MoveId moveId)
        {
            if (!moveId.IsValid)
            {
                Moves.SetCurrentMove(Moves.LastCurrentMove);
            }
            else
            {
                if (Moves.GetMoveText(moveId) == BlankTableEntry)
                {
                    Moves.SetCurrentMove(Moves.LastCurrentMove);
                }
                else
                {
                    Moves.SetCurrentMove(moveId);
                    IntInterface.CallBack.MoveSelect(moveId);
                }
            }
        }
    }
}
