using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using WeatherMaui.Models;

namespace WeatherMaui
{
    public partial class MainPage : ContentPage
    {
        readonly string WeatherApiKey;
        readonly short MilliSecondsBetweenSearch = 400;

        ObservableCollection<WeatherInfo> Data { get; set; } = null!;

        CancellationTokenSource? CancellationToken { get; set; }

        public MainPage()
        {
            var configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            WeatherApiKey = configuration.GetSection("WeatherApiKey").Get<string>();

            InitializeComponent();
        }

        public void ValueChange(object sender, TextChangedEventArgs e)
        {
            var TextProperty = (Entry)sender;
            _ = Search(TextProperty.Text);
        }

        void ChangeView(bool TurnOn)
        {
            LoadingIndicator.IsVisible = TurnOn;
            LoadingIndicator.IsRunning = TurnOn;

            List.IsVisible = !TurnOn;
        }

        async Task Search(string Name)
        {
            ChangeView(true);

            CancellationToken?.Cancel();

            CancellationToken = new();

            var Token = CancellationToken.Token;

            await Task.Delay(MilliSecondsBetweenSearch, Token);

            if (Token.IsCancellationRequested)
                return;

            var Value = Name.Trim();

            if (string.IsNullOrEmpty(Value))
            {
                ChangeView(false);
                return;
            }

            ChangeView(false);

        }
    }

}
