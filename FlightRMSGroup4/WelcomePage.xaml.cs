namespace FlightRMSGroup4;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    private async void OnClick_Main(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
