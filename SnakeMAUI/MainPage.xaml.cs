using System.Security.AccessControl;
using System.Timers;


namespace SnakeMAUI;


public partial class MainPage : ContentPage
{
    private int fieldSize;
    public MainPage()
    {
        InitializeComponent();
        fieldSize = 12;
    }

    private async void OnStartGame(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SnakePage(fieldSize));
    }

    private async void OnOptions(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Change field size", "Back", null, "8x8", "10x10", "12x12");
        switch (action) 
        {
            case "8x8":
                fieldSize = 8;
                break;
            case "10x10":
                fieldSize = 10;
                break;
            case "12x12":
                fieldSize = 12;
                break;
            default: 
                break;
        }
    }

    private async void OnAbout(object sender, EventArgs e)
    {
        await DisplayAlert("About me", "Hello, i am Kiryusha228 from 040 group of RSREU.\n" +
                           "My Github: https://github.com/Kiryusha228", "ОK");
    }

    private void OnExit(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}