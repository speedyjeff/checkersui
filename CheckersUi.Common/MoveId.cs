using System;

namespace CheckersUiWf
{
    // used internally for layout grid
    public class MoveId 
    {
        private int move = Config.InvalidMove; //Row number. First row = 1
        private CheckerColor color = CheckerColor.Invalid; //Column header color (Black or White)
        private MoveState position = MoveState.Before;

        public MoveId() { }
        public MoveId(int move, CheckerColor color, MoveState position = MoveState.Before)
        {
            Move = move;
            Color = color;
            Position = position;
        }
        public int Move { get => move; set => move = value; }
        public CheckerColor Color { get => color; set => color = value; }
        public MoveState Position { get => position; set => position = value; }
        public MoveId ShallowCopy() { return (MoveId)this.MemberwiseClone(); }
        public new string ToString()
        {
            return String.Format("Move {0} Color {1} Postion {2}", Move, Color, Position);
        }

        public MoveId(int row, string columnName, MoveState position = MoveState.Before)
        {
            Move = row + 1;
            Color = Column2Color(columnName);
            Position = position;
        }
        public MoveId(int row, int column, MoveState position = MoveState.Before)
        {
            Move = row + 1;
            Color = Column2Color(column > 0 ? Grid.Columns[column].Name : Config.MoveColumn);
            Position = position;
        }
        public String ColumnName { get => Color2ColumnName(color); set => color = Column2Color(value); }
        public int Row { get => move - 1; set => move = value + 1; }
        public bool IsValid
        {
            get =>
                    (move > 0 &&
                     color != CheckerColor.Invalid &&
                     move <= Moves.Count);
        }

        private static string Color2ColumnName(CheckerColor color)
        {
            if (color == CheckerColor.Black) return Config.BlackColumn;
            if (color == CheckerColor.White) return Config.WhiteColumn;
            return Config.MoveColumn;
        }

        private static CheckerColor Column2Color(string column)
        {
            if (column == Config.BlackColumn) return CheckerColor.Black;
            if (column == Config.WhiteColumn) return CheckerColor.White;
            return CheckerColor.Invalid;
        }
    }
}
