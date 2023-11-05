using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace weatherapp.features
{
    public class WeatherAPIModel : MonoBehaviour
    {
        [System.Serializable]
        public class DailyData
        {
            public List<string> time { get; set; }
            public List<double> temperature_2m_max { get; set; }
        }

        [System.Serializable]
        public class DailyUnits
        {
            public string Time { get; set; }
            public string Temperature2mMax { get; set; }
        }

        [System.Serializable]
        public class GeoTemperatureData
        {
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public float GenerationTimeMs { get; set; }
            public float UtcOffsetSeconds { get; set; }
            public string Timezone { get; set; }
            public string TimezoneAbbreviation { get; set; }
            public float Elevation { get; set; }
            public DailyUnits DailyUnits { get; set; }
            public DailyData Daily { get; set; }
        }
        
        public static GeoTemperatureData Parse(string json)
        {            
            return JsonConvert.DeserializeObject<GeoTemperatureData>(json);
        }

        public static WeatherAPIResult WeatherResult(GeoTemperatureData geoTemperatureData)
        {            
            return new WeatherAPIResult(geoTemperatureData.Daily.time[0], geoTemperatureData.Daily.temperature_2m_max[0]);
        }
    }
}