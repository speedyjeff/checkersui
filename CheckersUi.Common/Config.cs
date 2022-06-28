using System;

namespace CheckersUiWf
{
    public static class Config
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
        public const int FirstMoveTableRow = 1;

        //
        // Application Configuration
        //
        public static string AppName = "Checkers UI";
        public static int ClientSizeWidth = 1024;
        public static int ClientSizeHeight = 850 + 4; //includes menu, status, and padding/margin 
        public static int BoardWidth = 800;
        public static int BoardHeight = 800;
        public static int MovesWidth = 200;
        public static int MovesHeight = 800;
        public static int StatusHeight = 25;
        public static int MenuHeight = 25;

        //
        // Board Configuration
        //
        public static CheckerColor ColorOnTop = CheckerColor.Black; //board layout, which color top of screen
        public static string[] BoardNumbers =
        {
            "",
            "1","2","3","4","5","6","7","8",
            "9,","10","11","12","13","14","15","16",
            "17","18","19","20","21","22","23","24",
            "25","26","27","28","29","30","31","32"
        };

        //
        // Moves configuration
        //
        public static string MoveColumn = "#";
        public static string WhiteColumn = "White";
        public static string BlackColumn = "Black";
        public static int MoveColumnWidth = 25;
    }
}
