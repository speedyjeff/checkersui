### Winforms and Maui shell for a simple Checkers UI

![main](https://github.com/speedyjeff/checkersui/blob/master/Media/main.png)


## Visuals

Update the images under Media

## CheckersUiMaui

Maui application.

## CheckersUiWf

Winows Form Contol Library. 

External interface in Boundary.cs
Sub class CallBack to override events and support functions.

Playable squares are numbered 1 to 32.
Previous moves table accessed by move number and color (MoveId).

## Board

Initialization
```C#
Board = new Board(800 /* height */, 800 /* width */);
```

Call back on mouse click on a playable sqaure
```C#
public virtual void MouseClick(int square)
{
    Trace("MouseClick Square " + square.ToString());
}
```

Set the various states of the board
```C#
public enum CellState { Inative, Empty, White, Black, WhiteKing, BlackKing};
public enum HighLight { Selected, Target, None };
int selected = 15;
int take = 18;
int target = 22;
SetSquareHighLight(selected, HighLight.Selected);
SetSquareHighLight(target, HighLight.Target);
SetSquare(take, CellState.White, HighLight.None);
```

Add a swoop indicator on a move over a piece
```C#
int fromSquare = 15;
int toSquare = 22;
var index = AddSwoop(new Swoop(fromSquare, toSquare));

```

## Previous Moves

Initialization
```C#
Moves = new Moves(800 /* height */, 200 /* width */);
```
Call back for a selecting a move, by mouse or arrow keys
```C#
public virtual void MoveSelect(MoveId moveId)
{
    Trace(String.Format("MoveSelect move={0} column={1}",
          moveId.move, moveId.color ));
}
```

Add a row
```C#
Moves.AddMoveRow("11-15", "22-18");
```

Get the number of moves (row count)
```C#
int count = GetMoveCount();
```

Update a move
```C#
SetMoveText(new MoveId(2, BlackColumn, "(15x22)");
```

