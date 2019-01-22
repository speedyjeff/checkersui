using System;
using System.Windows.Forms;
using static CheckersUiWf.Boundary;

namespace CheckersUiWf
{
    public class CallBack
    {
        public CallBack() { }

        #region Utility Callbacks
        public virtual void Initialize()
        {
            Trace("Initialize");
        }
        public virtual void Trace(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        public virtual void Panic(string text)
        {
            Trace("Panic: " + text);
            throw new Exception(text);
        }
        #endregion
        #region Application Callbacks
        public virtual void OnKeyPress(char key)
        {
            Trace(String.Format("OnKeyPress key={0}", key ));
        }
        public virtual bool ProcessCmdKey(Keys keyData)
        {
            Trace("ProcessCmdKey " + keyData.ToString());
            bool rc = false; //pass on to be handled

            switch (keyData)
            {
                case Keys.Down:
                    {
                        MoveId moveId = GetCurrentMove();
                        if (moveId.move == InvalidMove) break;
                        if (moveId.move >= GetMoveCount()) break;
                        moveId.move++;
                        rc = true; //handled, do not pass on
                        if (GetMoveText(moveId) == "") break;
                        SetCurrentMove(moveId);
                        MoveSelect(moveId);
                        break;
                    }
                case Keys.Up:
                    {
                        MoveId moveId = GetCurrentMove();
                        if (moveId.move == InvalidMove) break;
                        if (moveId.move <= 1) break;
                        rc = true;
                        moveId.move--;
                        if (GetMoveText(moveId) == "") break;
                        SetCurrentMove(moveId);
                        MoveSelect(moveId);
                        break;
                    }
                case Keys.Left:
                    {
                        MoveId moveId = GetCurrentMove();
                        if (moveId.move == InvalidMove) break;
                        rc = true;
                        if (moveId.color == BlackColumn)
                        {
                            if (moveId.move <= 1) break;
                            moveId.move--;
                            moveId.color = WhiteColumn;
                            if (GetMoveText(moveId) == "") break;
                        }
                        else if (moveId.color == WhiteColumn)
                        {
                            moveId.color = BlackColumn;
                            if (GetMoveText(moveId) == "") break;
                        }
                        else if (moveId.color == MoveColumn)
                        {
                            moveId.color = WhiteColumn;
                            if (GetMoveText(moveId) == "") break;
                        }
                        SetCurrentMove(moveId);
                        MoveSelect(moveId);
                        break;
                    }
                case Keys.Right:
                    {
                        MoveId moveId = GetCurrentMove();
                        if (moveId.move == InvalidMove) break;
                        rc = true;
                        if (moveId.color == WhiteColumn)
                        {
                            if (moveId.move >= GetMoveCount()) break;
                            moveId.move++;
                            moveId.color = BlackColumn;
                            if (GetMoveText(moveId) == "") break;
                        }
                        else if (moveId.color == BlackColumn)
                        {
                            moveId.color = WhiteColumn;
                            if (GetMoveText(moveId) == "") break;
                        }
                        else if (moveId.color == MoveColumn)
                        {
                            moveId.color = BlackColumn;
                            if (GetMoveText(moveId) == "") break;
                        }
                        SetCurrentMove(moveId);
                        MoveSelect(moveId);
                        break;
                    }
            }

            return rc;
        }
        #endregion
        #region Board Callbacks
        public virtual void MouseClick(int square)
        {
            Trace("MouseClick Square " + square.ToString());
        }
        public virtual void MouseDoubleClick(int square)
        {
            Trace("MouseDoubleClick Square " + square.ToString());
        }
        public virtual void BoardClick(int row, int col, int square)
        {
            //Trace(string.Format("BoardClick: row {0} col {1} square {2}", row, col, square));
        }
        #endregion
        #region Board Support
        public virtual void NewGame()
        {
            if (BoardObj != null)
            {
                BoardObj.NewGame();
            }
        }
        public virtual void SetSquare(int square, CellState state, HighLight highlight)
        {
            if (BoardObj != null)
            {
                BoardObj.SetSquare(square, state, highlight);
            }
        }
        public virtual CellState GetSquareState(int square)
        {
            CellState state = CellState.Empty;
            if (BoardObj != null)
            {
                state = BoardObj.GetSquareState(square);
            }
            return state;
        }
        public virtual void SetSquareState(int square, CellState state)
        {
            if (BoardObj != null)
            {
                BoardObj.SetSquareState(square, state);
            }
        }
        public virtual HighLight GetSquareHighLight(int square)
        {
            HighLight state = HighLight.None;
            if (BoardObj != null)
            {
                state = BoardObj.GetSquareHighLight(square);
            }
            return state;
        }
        public virtual void SetSquareHighLight(int square, HighLight hightlight)
        {
            if (BoardObj != null)
            {
                BoardObj.SetSquareHighLight(square, hightlight);
            }
        }
        public virtual int AddSwoop(int fromSquare, int toSquare)
        {
            int swoop = InvalidSwoop;
            if (BoardObj != null)
            {
                swoop = BoardObj.AddSwoop(fromSquare, toSquare);
            }
            return swoop;
        }
        public virtual bool RemoveSwoop(int index)
        {
            bool rc = false;
            if (BoardObj != null)
            {
                rc = BoardObj.RemoveSwoop(index);
            }
            return rc;
        }
        public virtual void RemoveAllSwoops()
        {
            if (BoardObj != null)
            {
                BoardObj.RemoveAllSwoops();
            }
        }
        #endregion
        #region Move Callbacks
        public virtual bool MoveClick (MoveId moveId)
        {
            bool rc = false;
            if ((moveId.color == BlackColumn || moveId.color == WhiteColumn) &&
                (GetMoveText(moveId) != ""))
            {
                MoveSelect(moveId);
                rc = true;
            }
            return rc;
        }
        public virtual void MoveSelect(MoveId moveId)
        {
            Trace(String.Format("MoveSelect move={0} column={1}",
                moveId.move, moveId.color ));
        }
        #endregion
        #region Moves Support
        public virtual void SetMoveText(MoveId moveId, string value)
        {
            if (MovesObj != null)
            {
                MovesObj.SetMoveText(moveId, value);
            }
        }
        public virtual string GetMoveText(MoveId moveId)
        {
            string text = "";
            if (MovesObj != null)
            {
                text = MovesObj.GetMoveText(moveId);
            }
            return text;
        }
        public virtual void AddMoveRow(string black, string white)
        {
            if (MovesObj != null)
            {
                MovesObj.AddRow(black, white);
            }
        }
        public virtual void SetCurrentMove(MoveId moveId)
        {
            if (MovesObj != null)
            {
                MovesObj.SetCurrentMove(moveId);
            }
        }
        public virtual MoveId GetCurrentMove()
        {
            MoveId moveId = new MoveId();
            if (MovesObj != null)
            {
                moveId = MovesObj.GetCurrentMove();
            }
            return moveId;
        }
        public virtual int GetMoveCount()
        {
            int count = -1;
            if (MovesObj != null)
            {
                count = MovesObj.Count;
            }
            return count;
        }
        #endregion

        internal Board BoardObj = null;
        internal Moves MovesObj = null;
    }
}
