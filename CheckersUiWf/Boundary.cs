using System;

namespace CheckersUiWf
{
    /// <summary>
    /// Common external interface
    /// </summary>
    public static partial class Boundary
    {
        //Configuration
        public static string AppName = "Checkers UI";
        public static bool WhiteOnTop = false; //board layout, which color top of screen
        public static int ClientSizeWidth = 1024;
        public static int ClientSizeHeight = 824;
        public static int BoardWidth = 800;
        public static int BoardHeight = 800;
        public static int MovesWidth = 200;
        public static int MovesHeight = 800;
    }

    /// <summary>
    /// Checkers external interface
    /// </summary>
    public enum CellState { Inactive, Empty, White, Black, WhiteKing, BlackKing};
    public enum HighLight { Selected, Target, None };
    public static partial class Boundary
    {
        //Update the following with your sub class of CallBack
        public static CallBack_Checkers CheckersCallBack = new CallBack_Checkers();

        //Constants
        public const int BoardRowCount = 8;
        public const int BoardColumnCount = 8;
        public const int NumberSquares = 32;
        public const int NumberEachTeam = 12;
        public const int InvalidSquare = -1;
        public const int InvalidSwoop = -1;
    }

    /// <summary>
    /// Moves external interface
    /// </summary>
    public static partial class Boundary
    {
        //Update the following with your sub class of CallBack
        public static CallBack_Moves MovesCallBack = new CallBack_Moves();

        //Configuration
        public static string Word4White = "White";
        public static string Word4Black = "Black";
    }
}
