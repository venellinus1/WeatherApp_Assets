using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
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

        public static WeatherLocationModel ParseByIP(string json)
        {
            return JsonConvert.DeserializeObject<WeatherLocationModel>(json);
        }
    }

    
}