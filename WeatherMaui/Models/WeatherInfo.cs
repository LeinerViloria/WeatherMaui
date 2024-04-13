
namespace WeatherMaui.Models
{
    public class WeatherInfo
    {
        public WeatherData Data { get; set; } = null!;
        public WeatherLocation Location { get; set; } = null!;
    }

    public class WeatherData
    {
        public string Time { get; set; } = string.Empty;
        public WeatherDataResume Values { get; set; } = null!;
    }

    public class WeatherDataResume
    {
        public int Humidity { get; set; }
        public float Temperature { get; set; }
    }

    public class  WeatherLocation
    {
        public string Name { get; set; } = null!;
    }
}
