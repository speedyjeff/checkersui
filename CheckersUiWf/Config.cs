using System;
using System.Windows.Forms;

namespace CheckersUiWf
{
    public static class Config
    {
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
        // Move table navigation keys
        public static Keys MoveUp = Keys.Up;
        public static Keys MoveDown = Keys.Down;
        public static Keys MoveLeft = Keys.Left;
        public static Keys MoveRight = Keys.Right;
        public static Keys MoveLeftPosition = Keys.B;
        public static Keys MoveRightPosition = Keys.M;
        public static Keys MoveFirst = Keys.Home;
        public static Keys MoveLast = Keys.End;
    }
}
