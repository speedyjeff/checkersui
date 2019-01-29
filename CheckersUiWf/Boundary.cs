using System;

namespace CheckersUiWf
{
    /// <summary>
    /// External interface
    /// </summary>
    public static partial class Boundary
    {
        //Update the following with your sub class of CallBack
        public static CallBack gCallBack = new CallBack();

        //Application Configuration
        public static string AppName = "Checkers UI";
        public static bool WhiteOnTop = false; //board layout, which color top of screen
        public static int ClientSizeWidth = 1024;
        public static int ClientSizeHeight = 850 + 4; //includes menu, status, and padding/margin 
        public static int BoardWidth = 800;
        public static int BoardHeight = 800;
        public static int MovesWidth = 200;
        public static int MovesHeight = 800;
        public static int StatusHeight = 25;
        public static int MenuHeight = 25;

        /// <summary>
        /// Board external interface
        /// </summary>
        public enum CellState { Inactive, Empty, White, Black, WhiteKing, BlackKing};
        public enum HighLight { Selected, Target, None };

        //Constants
        public const int BoardRowCount = 8;
        public const int BoardColumnCount = 8;
        public const int NumberSquares = 32;
        public const int NumberEachTeam = 12;
        public const int InvalidSquare = -1;
        public const int InvalidSwoop = -1;

        /// <summary>
        /// Moves external interface
        /// </summary>
        public struct MoveId
        {
            public int move;        //Row number. First row = 1
            public string color;    //Column header color (Black or White)

            public MoveId(int imove = InvalidMove, string icolor = "")
            {
                move = imove;
                color = icolor;
            }
        }

        //Configuration
        public static string MoveColumn = "#";
        public static string WhiteColumn = "White";
        public static string BlackColumn = "Black";
        public static int MoveColumnWidth = 25;
        public const int InvalidMove = -1;

        /// <summary>
        /// Menus external interface
        /// </summary>
        public const string OpenMenuItemName = "OpenMenuItemName";
        public const string ExitMenuItemName = "ExitMenuItemName";

    }
}
