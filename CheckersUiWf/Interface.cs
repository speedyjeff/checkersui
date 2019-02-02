using System;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;
using static CheckersUiWf.IntInterface;

namespace CheckersUiWf
{
    //
    // Common
    //
    public enum CheckerColor { Black, White, Invalid };
    public enum CellState { Inactive, Empty, White, Black, WhiteKing, BlackKing};
    public enum HighLight { Selected, Target, None };
    public enum Positions { Start, End } //Beginning or end of move

    public partial class MoveId // location in moves table
    {
        private int move = InvalidMove; //Row number. First row = 1
        private CheckerColor color = CheckerColor.Invalid; //Column header color (Black or White)
        private Positions position = Positions.Start;

        public MoveId() { }
        public MoveId(int move, CheckerColor color, Positions position = Positions.Start)
        {
            Move = move;
            Color = color;
            Position = position;
        }
        public int Move { get => move; set => move = value; }
        public CheckerColor Color { get => color; set => color = value; }
        public Positions Position { get => position; set => position = value; }
        public MoveId ShallowCopy() { return (MoveId)this.MemberwiseClone(); }
        public new string ToString() { return String.Format("Move {0} Color {1} Postion {2}", Move, Color, Position);
        }
    }

    //
    // External interface
    //
    public static class ExtInterface
    {
        //Constants
        public const int BoardRowCount = 8;
        public const int BoardColumnCount = 8;
        public const int NumberSquares = 32;
        public const int NumberEachTeam = 12;
        public const int InvalidMove = -1;
        public const int InvalidSquare = -1;
        public const int InvalidSwoop = -1;
        public const string BlankTableEntry = "";

        //
        // Application
        //
        public static void SetCallBack(CallBackCls callback)
        {
            CallBack = callback;
        }
        //
        // Status bars
        //
        public static void UpdateLeftStatus(string text)
        {
            if (Checkers != null) Checkers.UpdateLeftStatus(text);
        }
        public static void UpdateRightStatus(string text)
        {
            if (Checkers != null) Checkers.UpdateRightStatus(text);
        }
        //
        // Board
        //
        public static void NewGame()
        {
            if (Board != null) Board.NewGame();
        }
        public static void SetSquare(int square, CellState state, HighLight highlight)
        {
            if (Board != null) Board.SetSquare(square, state, highlight);
        }
        public static CellState GetSquareState(int square)
        {
            return (Board == null ? CellState.Empty :  Board.GetSquareState(square));
        }
        public static void SetSquareState(int square, CellState state)
        {
            if (Board != null) Board.SetSquareState(square, state);
        }
        public static HighLight GetSquareHighLight(int square)
        {
            return (Board == null ? HighLight.None : Board.GetSquareHighLight(square));
        }
        public static void SetSquareHighLight(int square, HighLight hightlight)
        {
            if (Board != null) Board.SetSquareHighLight(square, hightlight);
        }
        public static int AddSwoop(int fromSquare, int toSquare)
        {
            return (Board == null ? InvalidSwoop : Board.AddSwoop(fromSquare, toSquare));
        }
        public static bool RemoveSwoop(int index)
        {
            return (Board == null ? false : Board.RemoveSwoop(index));
        }
        public static void RemoveAllSwoops()
        {
            if (Board != null) Board.RemoveAllSwoops();
        }
        public static void UpdateCellNumbers(string[] cellNumbers)
        {
            if (Board != null) Board.CellNumbers = cellNumbers;
        }
        public static void SetColorOnTop(CheckerColor color)
        {
            if (Board != null) Board.SetColorOnTop(color);
        }
        //
        // Previous Move Table
        //
        public static void SetMoveText(MoveId moveId, string value)
        {
            if (Moves != null) Moves.SetMoveText(moveId, value);
        }
        public static string GetMoveText(MoveId moveId)
        {
            return (Moves == null ? "" : Moves.GetMoveText(moveId));
        }
        public static void AddMoveRow(string black, string white)
        {
            if (Moves != null) Moves.AddRow(black, white);
        }
        public static void SetCurrentMove(MoveId moveId)
        {
            if (Moves != null) Moves.SetCurrentMove(moveId);
        }
        public static MoveId GetCurrentMove()
        {
            return (Moves == null ? new MoveId() : Moves.GetCurrentMove());
        }
        public static int GetMoveCount()
        {
            return (Moves == null ? -1 :  Moves.Count);
        }
    }

    //
    // Internal Interfaces
    //
    internal enum MoveDirection { Left, Right, Up, Down, LeftPosition, RightPosition, OtherKey }

    internal static class IntInterface
    {
        internal struct Location
        {
            public int Row;
            public int Col;
        }
        internal struct Swoop
        {
            public Location From;
            public Location To;
        }

        internal static CallBackCls CallBack = new CallBackCls();
        internal static CheckersFrm Checkers;
        internal static BoardUct Board;
        internal static MovesUct Moves;
        internal static StatusStrip StatusStripR;
        internal static StatusStrip StatusStripL;
        internal static DataGridView Grid;
    }

}
