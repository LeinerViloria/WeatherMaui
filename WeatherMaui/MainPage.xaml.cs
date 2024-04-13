using Microsoft.Extensions.Configuration;

namespace WeatherMaui
{
    public partial class MainPage : ContentPage
    {
        readonly string WeatherApiKey;
        readonly short MilliSecondsBetweenSearch = 400;

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

        async Task Search(string Name)
        {
            CancellationToken?.Cancel();

            CancellationToken = new();

            var Token = CancellationToken.Token;

            await Task.Delay(MilliSecondsBetweenSearch, Token);

            if (Token.IsCancellationRequested)
                return;

            var Value = Name.Trim();

            if (string.IsNullOrEmpty(Value))
                return;

        }
    }

}
