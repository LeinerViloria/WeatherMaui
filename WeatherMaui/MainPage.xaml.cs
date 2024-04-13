using Microsoft.Extensions.Configuration;

namespace WeatherMaui
{
    public partial class MainPage : ContentPage
    {
        string WeatherApiKey { get; set; }

        public MainPage()
        {
            var configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            WeatherApiKey = configuration.GetSection("WeatherApiKey").Get<string>();

            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
        }
    }

}
