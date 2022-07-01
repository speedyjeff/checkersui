namespace CheckersUiMaui;

public partial class NewGamePage : ContentPage
{
	public NewGamePage()
	{
		InitializeComponent();
	}

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // clear the details for navigation
        var parameters = new Dictionary<string, object>()
        {
            {"OpenFileNamePath", "" },
            {"NavigationAction", "reset" }
        };

        // navigate to a new game
        await Shell.Current.GoToAsync("//MainPage", animate: true, parameters);
    }
}