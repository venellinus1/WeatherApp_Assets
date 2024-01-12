using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

namespace weatherapp.features
{
    public class WeatherLocationModel : IWeatherLocation
    {

        private readonly float _longitude;
        private readonly float _latitude;

        public WeatherLocationModel(float longitude, float latitude)
        {
            ValidateLongitude(longitude);
            ValidateLatitude(latitude);

            _longitude = longitude;
            _latitude = latitude;
        }

        public float Longitude
        {
            get { return _longitude; }
        }

        public float Latitude
        {
            get { return _latitude; }
        }

        private void ValidateLongitude(float longitude)
        {
            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
            }
        }

        private void ValidateLatitude(float latitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
            }
        }


        public static WeatherLocationModel ParseByIP(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<WeatherLocationModel>(json);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Invalid JSON format.", ex);
            }
        }
    }

    
}