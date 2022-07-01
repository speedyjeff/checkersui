namespace CheckersUiMaui;

public partial class DataGridControl : ContentView
{
	public DataGridControl()
	{
		InitializeComponent();
        CurrentRow = 1;
    }

    public void Clear()
    {
        // reset back to just the header

        // remove the row definitions back down to 1
        MainGrid.RowDefinitions.Clear();
        MainGrid.RowDefinitions.Add(new RowDefinition());

        // remove all elements but the header
        var elements = MainGrid.Children.ToList();
        foreach(var elem in elements)
        {
            var row = MainGrid.GetRow(elem);
            if (row >= 1)
            {
                // remove it
                MainGrid.Children.Remove(elem);
            }
        }

        // reset the current row
        CurrentRow = 1;
    }

	public void AddRow(string column1, string column2)
	{
		// add a row
		MainGrid.RowDefinitions.Add(new RowDefinition());

		// add column 0
		var frame0 = new Frame()
		{
			BorderColor = Colors.Black,
			CornerRadius = 0,
			Content = new Label() { Text = $"{CurrentRow}" }
		};
		MainGrid.SetRow(frame0, CurrentRow);
        MainGrid.SetColumn(frame0, 0);
		MainGrid.Children.Add(frame0);

        // add column 1
        var frame1 = new Frame()
        {
            BorderColor = Colors.Black,
            CornerRadius = 0,
            Content = new Label() { Text = column1 },
        };
        MainGrid.SetRow(frame1, CurrentRow);
        MainGrid.SetColumn(frame1, 1);
        MainGrid.Children.Add(frame1);

        // add column 2
        var frame2 = new Frame()
        {
            BorderColor = Colors.Black,
            CornerRadius = 0,
            Content = new Label() { Text = column2 },
        };
        MainGrid.SetRow(frame2, CurrentRow);
        MainGrid.SetColumn(frame2, 2);
        MainGrid.Children.Add(frame2);

        // increment row
        CurrentRow++;
	}

	#region private
	private int CurrentRow = 1;
	#endregion
}