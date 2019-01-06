### Winforms shell for a simple Checkers UI

![main](https://github.com/speedyjeff/checkersui/blob/master/Media/main.png)


## Visuals

Update the images under CheckersUi\Media

## Board

Initialization
```C#
Board = new Board(800 /* height */, 800 /* width */);
Board.OnSelected += Board_OnSelected;

private void Board_OnSelected(int row, int col)
{
    System.Diagnostics.Debug.WriteLine("Cell {0},{1} selected", row, col);
}
```

Set the various states of the board
```C#
public enum CellState { Inative, Active, Red, Black, RedKing, BlackKing, Selected };
Board.SetCellState(row, col, CellState.Selected);
```

Get Cell state
```C#
CellState state = Board.GetCellState(row, col);
```

Add a swoop indicator on a move over a piece
```C#
var index = Board.AddSwoop(new Swoop()
{
    From = new Location() { Row = 1, Col = 3 },
    To = new Location() { Row = 3, Col = 1 }
});
```

Get a active swoop
```C#
var swoop = Board.GetSwoop(index);
```

Remove a swoop
```C#
if (Board.RemoveSwoop(index)) ...
```

## Previous Moves

Initialization
```C#
Moves = new Moves(800 /* height */, 200 /* width */);
```

Add a row
```C#
string red = "";
string black = "";
Moves.AddRow(red, black);
```

Get the row count
```C#
int count = Moves.Count;
```

Update a row
```C#
string red = "";
string black = "";
Moves.UpdateRow(Moves.Count - 1, red, black);
```

Get row content
```C#
string red = "";
string black = "";
Moves.GetRow(Moves.Count - 1, out red, out black);
```