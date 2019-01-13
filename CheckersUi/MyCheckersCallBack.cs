using CheckersUiWf;
using static CheckersUiWf.Boundary;

namespace CheckersUi
{
    public class MyCheckersCallBack: CallBack_Checkers
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
            SetSquareHighLight(Selected, HighLight.Selected);
            SetSquareHighLight(Target, HighLight.Target);
            Swoop = AddSwoop(Selected, Target);
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

        public override void OnKeyPressed(char key)
        {
            Trace("OnKeyPressed " + key.ToString());

            if (key == 'x' && Selected != InvalidSquare)
            {
                SetSquare(Selected, CellState.Empty, HighLight.None);
                Selected = InvalidSquare;
                if (Swoop != InvalidSwoop)
                {
                    RemoveSwoop(Swoop);
                    Swoop = InvalidSwoop;
                }
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

        private int Selected = 7;
        private int Target = 14;
        private int Swoop = InvalidSwoop;
    }
}
