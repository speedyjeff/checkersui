### Winforms shell for a simple Checkers UI

## Board

Initialization
```
Board = new Board(800 /* height */, 800 /* width */);
Board.OnSelected += Board_OnSelected;

private void Board_OnSelected(int row, int col)
{
    System.Diagnostics.Debug.WriteLine("Cell {0},{1} selected", row, col);
}
```

Set the various states of the board
```
public enum CellState { Inative, Active, Red, Black, RedKing, BlackKing, Selected };
Board.SetCellState(row, col, CellState.Selected);
```

## Previous Moves

Initialization
```
Moves = new Moves(800 /* height */, 200 /* width */);
```

Add a row
```
string red = "";
string black = "";
Moves.AddRow(red, black);
```

Get the row count
```
int count = Moves.Count;
```

Update a row
```
string red = "";
string black = "";
Moves.UpdateRow(Moves.Count - 1, red, black);
```

Get row content
```
string red = "";
string black = "";
Moves.GetRow(Moves.Count - 1, out red, out black);
```