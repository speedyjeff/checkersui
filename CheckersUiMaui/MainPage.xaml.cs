using System;

namespace CheckersUiMaui
{
    [QueryProperty(nameof(OpenFileNamePath), "OpenFileNamePath")]
    [QueryProperty(nameof(NavigationAction), "NavigationAction")]
    public partial class MainPage : ContentPage
    {
        public const int BoardWidthCells = 8;
        public const int BoardHeightCells = 8;
        public const int PlayerRows = 3;

        public MainPage()
        {
            InitializeComponent();

            Selected = new Cell();
            Targeted = new Cell();

            // setup handlers
            var number = 0;
            for(int row = 0; row<BoardHeightCells; row++)
            {
                for(int col=0; col<BoardWidthCells; col++)
                {
                    // get the control
                    var control = GetCellControl(row, col);

                    // get the initial state
                    var state = CellState.Inactive;
                    if ((row % 2 == 0 && col % 2 != 0) || (row % 2 != 0 && col % 2 == 0))
                    {
                        // default empty
                        state = CellState.Empty;
                    }

                    // setup
                    if (control != null)
                    {
                        // inialize
                        control.OnTouch += CellControl_OnTouch;
                        control.Row = row;
                        control.Column = col;
                        control.Number = state != CellState.Inactive ? ++number : 0;
                        control.SetCellState(state);
                    }
                    else
                    {
                        // unknown
                        System.Diagnostics.Debug.WriteLine($"missed grid {row}x{col}");
                    }
                }
            }
        }

        // used when navigating
        public string OpenFileNamePath { get; set; }
        public string NavigationAction { get; set; }

        #region private
        private Cell Selected;
        private Cell Targeted;

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            // todo when clicking 'home' the board is cleared

            base.OnNavigatedTo(args);

            if (!string.IsNullOrWhiteSpace(OpenFileNamePath) && File.Exists(OpenFileNamePath))
            {
                // todo load from a file

                // initalize the swoop
                Swoop.ClearSwoop();

                // clear the datagrid
                DataGrid.Clear();

                Selected = new Cell();
                Targeted = new Cell();
            }
            else
            {
                // initialize the grid
                for (int row = 0; row < BoardHeightCells; row++)
                {
                    for (int col = 0; col < BoardWidthCells; col++)
                    {
                        // get the control
                        var control = GetCellControl(row, col);

                        if (control != null)
                        {
                            if (control.State != CellState.Inactive)
                            {
                                // get the initial state
                                var state = CellState.Empty;
                                // initial piece placement
                                if (row < PlayerRows)
                                {
                                    state = CellState.Black;
                                }
                                else if (row >= BoardHeightCells - PlayerRows)
                                {
                                    state = CellState.Red;
                                }

                                // set starting state
                                control.SetHighLight(HighLight.None);
                                control.SetCellState(state);
                            }
                        }
                        else
                        {
                            // unknown
                            System.Diagnostics.Debug.WriteLine($"missed grid {row}x{col}");
                        }
                    }
                }

                // initalize the swoop
                Swoop.ClearSwoop();

                // clear the datagrid
                DataGrid.Clear();

                Selected = new Cell();
                Targeted = new Cell();

                // debug only
                {
                    // setup an initial board configuration

                    // move black
                    Grid_2_5.SetCellState(CellState.Empty);
                    Grid_3_4.SetCellState(CellState.Black);

                    // move red
                    Grid_5_2.SetCellState(CellState.Empty);
                    Grid_4_3.SetCellState(CellState.Red);

                    // setup the selection
                    Selected = new Cell() { Row = 3, Column = 4 };
                    Targeted = new Cell() { Row = 5, Column = 2 };

                    // add swoop
                    Swoop.AddSwoop(from: Selected, to: Targeted);

                    // add highlights
                    Grid_3_4.SetHighLight(HighLight.Selected);
                    Grid_5_2.SetHighLight(HighLight.Target);

                    // update table
                    DataGrid.AddRow("11-15", "11-28");
                    DataGrid.AddRow("(15x22)", "");

                    // update labels
                    Status.Text = "Black to move";
                    Totals.Text = "Black (12) Red (12)";
                }
                // debug only
            }
        }

        private CellControl GetCellControl(int row, int col)
        {
            return (CellControl)FindByName($"Grid_{row}_{col}");
        }

        private void CellControl_OnTouch(Cell to)
        {
            // clear the swoop
            Swoop.ClearSwoop();

            // unhighlight the previous selection
            var prv = GetCellControl(Targeted.Row, Targeted.Column);
            prv.SetHighLight(HighLight.None);

            // check if this is a valid destination
            var dst = GetCellControl(to.Row, to.Column);
            if (dst.State == CellState.Inactive || 
                (to.Row == Selected.Row && to.Column == Selected.Column))
            {
                // nothing
                System.Diagnostics.Debug.WriteLine($"invalid cell {to.Row}x{to.Column}");
            }
            else
            {
                // this is valid, so add the swoop
                Swoop.AddSwoop(Selected, to);
                // highlight this cell
                dst.SetHighLight(HighLight.Target);
                // set the current target
                Targeted = to;
            }
        }
        #endregion
    }
}