using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersUiWf
{
    /*
    public static class ExtInterface
    {

        //
        // Application
        //
        public static void SetCallBack(CallBack callback)
        {
            IntInterface.CallBack = callback;
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
        public static void ClearBoard()
        {
            if (Board != null) Board.ClearBoard();
        }
        public static void SetSquare(int square, CellState state, HighLight highlight)
        {
            if (Board != null) Board.SetSquare(square, state, highlight);
        }
        public static CellState GetSquareState(int square)
        {
            return (Board == null ? CellState.Empty : Board.GetSquareState(square));
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
        public static void ClearMoves()
        {
            if (Moves != null) Moves.ClearMoves();
        }
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
        public static void SetCurrentMove(MoveDirection direction)
        {
            if (Moves != null) Moves.SetCurrentMove(direction);
        }
        public static MoveId GetCurrentMove()
        {
            return (Moves == null ? new MoveId() : Moves.GetCurrentMove());
        }
        public static int GetMoveCount()
        {
            return (Moves == null ? -1 : Moves.Count);
        }
    }
    */
}
