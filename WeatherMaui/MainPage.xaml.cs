﻿using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using WeatherMaui.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace WeatherMaui
{
    public partial class MainPage : ContentPage
    {
        readonly IHttpClientFactory ClientFactory;
        readonly string WeatherApiKey;
        readonly string RequestUrl = "https://api.tomorrow.io/v4/weather/realtime";
        readonly short MilliSecondsBetweenSearch = 400;

        ObservableCollection<WeatherInfo> Data { get; set; } = null!;

        CancellationTokenSource? CancellationToken { get; set; }

        public MainPage()
        {
            ClientFactory = MauiProgram.Services.GetService<IHttpClientFactory>()!;
            var configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            WeatherApiKey = configuration.GetSection("WeatherApiKey").Get<string>()!;

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

            await SendRequest(Value);

            ChangeView(false);

        }

        public async Task SendRequest(string Value)
        {
            using(var client = ClientFactory.CreateClient())
            {
                var RequestUri = $"{RequestUrl}?location={Value}&apikey={WeatherApiKey}";
                var Request = await client.GetAsync(RequestUri);

                var Message = await Request.Content.ReadAsStringAsync();

                if (Request.StatusCode != HttpStatusCode.OK)
                {
                    var Error = JsonConvert.DeserializeObject<ErrorRequestDTO>(Message)!;
                    _ = DisplayAlert("Error al traer la información", Error.Message, "Cerrar");
                    return;
                }

                var Result = JsonConvert.DeserializeObject<WeatherInfo>(Message)!;
            }
        }
    }

}
