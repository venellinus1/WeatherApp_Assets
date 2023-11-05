
namespace weatherapp.features
{
    public class WeatherLocationModel : IWeatherLocation
    {
        public WeatherLocationModel(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
    public interface IWeatherLocation
    {
        float Latitude { get; set; }
        float Longitude { get; set; }
    }

    
}