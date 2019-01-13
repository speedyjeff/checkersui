using static CheckersUiWf.Boundary;

namespace CheckersUiWf
{
    public class CallBack_Checkers : CallBack
    {
        public CallBack_Checkers()
        {
        }

        /// <summary>
        /// Checkers board call backs
        /// </summary>

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

        public virtual void OnKeyPressed(char key)
        {
            Trace("OnKeyPressed " + key.ToString());
        }

        /// <summary>
        /// Checkers support methods
        /// </summary>
        internal Board BoardObj = null;

        public void NewGame()
        {
            if (BoardObj != null)
            {
                BoardObj.NewGame();
            }
        }

        public void SetSquare(int square, CellState state, HighLight highlight)
        {
            if (BoardObj != null)
            {
                BoardObj.SetSquare(square, state, highlight);
            }
        }

        public CellState GetSquareState(int square)
        {
            CellState state = CellState.Empty;
            if (BoardObj != null)
            {
                state = BoardObj.GetSquareState(square);
            }
            return state;
        }

        public void SetSquareState(int square, CellState state)
        {
            if (BoardObj != null)
            {
                BoardObj.SetSquareState(square, state);
            }
        }

        public HighLight GetSquareHighLight(int square)
        {
            HighLight state = HighLight.None;
            if (BoardObj != null)
            {
                state = BoardObj.GetSquareHighLight(square);
            }
            return state;
        }

        public void SetSquareHighLight(int square, HighLight hightlight)
        {
            if (BoardObj != null)
            {
                BoardObj.SetSquareHighLight(square, hightlight);
            }
        }

        public int AddSwoop(int fromSquare, int toSquare)
        {
            int swoop = InvalidSwoop;
            if (BoardObj != null)
            {
                swoop = BoardObj.AddSwoop(fromSquare, toSquare);
            }
            return swoop;
        }

        public bool RemoveSwoop(int index)
        {
            bool rc = false;
            if (BoardObj != null)
            {
                rc = BoardObj.RemoveSwoop(index);
            }
            return rc;
        }

    }
}
