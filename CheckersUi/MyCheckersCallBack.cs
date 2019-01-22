using System;
using CheckersUiWf;
using System.Windows.Forms;
using static CheckersUiWf.Boundary;

namespace CheckersUi
{
    public class MyCheckersCallBack: CallBack
    {
        public MyCheckersCallBack() { }

        public override void Trace(string text)
        {
            System.Diagnostics.Debug.WriteLine("MyCallBack: " + text);
        }

        public override void Initialize()
        {
            base.Initialize();

            Trace("MyCallBack Initialize");

            NewGame();
            SetSquareState(15, CellState.Black);
            SetSquareState(11, CellState.Empty);
            SetSquareState(18, CellState.White);
            SetSquareState(22, CellState.Empty);
            SetSquareHighLight(Selected, HighLight.Selected);
            SetSquareHighLight(Target, HighLight.Target);
            Swoop = AddSwoop(Selected, Target);
            SetMoveText(new MoveId(1, BlackColumn), "11-15");
            SetMoveText(new MoveId(1, WhiteColumn), "22-18");
            SetMoveText(new MoveId(2, BlackColumn), "(15x22)");
            SetCurrentMove(new MoveId(2, BlackColumn));
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
            }
        }

        public override void MoveSelect(MoveId moveId)
        {
            Trace(String.Format("MoveSelect move={0} column={1}",
                moveId.move, moveId.color));
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

        private int Selected = 15;
        private int Target = 22;
        private int Swoop = InvalidSwoop;
    }
}
