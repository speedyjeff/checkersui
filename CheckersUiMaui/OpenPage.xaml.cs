namespace CheckersUiMaui;

public partial class OpenPage : ContentPage
{
	public OpenPage()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		var result = await FilePicker.PickAsync();

		// todo do the work to load a model

		// load the paramters with details of what was loaded
		var parameters = new Dictionary<string, object>()
		{
			{"OpenFileNamePath", result.FullPath },
			{"NavigationAction", "reset" }
		};

        // navigate to a new game
        await Shell.Current.GoToAsync("//MainPage", animate: true, parameters);
    }
}