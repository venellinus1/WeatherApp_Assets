namespace weatherapp.features
{
    public class WeatherAPIResult
    {
        public WeatherAPIResult(string dailyTime0, decimal dailyTemperature2mMax0)
        {
            DailyTime0 = dailyTime0;
            DailyTemperature2mMax0 = dailyTemperature2mMax0;
        }

        public string DailyTime0 { get; set; }
        public decimal DailyTemperature2mMax0 { get; set; }
    }
}
