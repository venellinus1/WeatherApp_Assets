using UnityEngine;
using UnityEngine.UI;
using weatherapp.features;
using venelin.androidutils;
using System.Collections;
using Newtonsoft.Json;
namespace weatherapp.main
{
    public class MainService : MonoBehaviour 
    {
        public Button checkLocationButton;
        private WeatherLocationService weatherLocationService;
        private WeatherAPIService weatherAPIService;
        private void Start()
        {
            checkLocationButton.onClick.AddListener(OnButtonClick);

            InitializeLocationService();
            InitializeWeatherAPIService();
        }

        private void InitializeLocationService()
        {
            weatherLocationService = gameObject.AddComponent<WeatherLocationService>();
            weatherLocationService.OnComplete += (locationData) => {
                OnLocationReadComplete(locationData);
            };
            weatherLocationService.OnFail += () => OnLocationReadFail();
        }

        private void InitializeWeatherAPIService()
        {
            weatherAPIService = gameObject.AddComponent<WeatherAPIService>();
            weatherAPIService.OnComplete += (weatherDataJson) => {
                OnWeatherAPIReadComplete(weatherDataJson);
            };
            weatherAPIService.OnFail += () => OnWeatherAPIReadFail();
        }
        private void OnWeatherAPIReadComplete(string jsonData)
        {            
            WeatherAPIResult temperatureResult = WeatherAPIModel.WeatherResult(WeatherAPIModel.Parse(jsonData));
            Toast.ShowToast($"Temperature for day {temperatureResult.DailyTime0} is {temperatureResult.DailyTemperature2mMax0}");
        }

        private void OnWeatherAPIReadFail()
        {
            Toast.ShowToast("Weather API Failed Read");
        }

        private void OnButtonClick()
        {
            CheckLocation();
        }
        private void CheckLocation()
        {
            weatherLocationService.StartService();
        }
        private void OnLocationReadComplete(WeatherLocationModel location)
        {
            weatherAPIService.StartService(location);
        }
        private void OnLocationReadFail()
        {
            Toast.ShowToast($"problem reading location");
        }

    }


}

