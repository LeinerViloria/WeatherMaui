using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WeatherMaui
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; } = null!;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var a = Assembly.GetExecutingAssembly();

            using var stream = a.GetManifestResourceStream("WeatherMaui.appsettings.json");

            var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

            builder.Configuration.AddConfiguration(config);

            var app = builder.Build();

            Services = app.Services;

            return app;
        }
    }
}
