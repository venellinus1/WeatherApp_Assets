namespace weatherapp.features
{
    public class WeatherLocationModel
    {
        public WeatherLocationModel(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
}