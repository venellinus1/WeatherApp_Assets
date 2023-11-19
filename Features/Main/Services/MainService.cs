using UnityEngine;
using UnityEngine.UI;
using weatherapp.features;
using venelin.androidutils;


namespace weatherapp.main
{
    public class MainService : MonoBehaviour
    {
       

        public Button checkLocationButton;       
        private WeatherLocationService weatherLocationService;
        private WeatherAPIService weatherAPIService;
        private LocationByIPService locationByIPService;
        private IPService IPService;
        private void Start()
        {
            //checkLocationButton.onClick.AddListener(OnButtonClick);
            checkLocationButton.onClick.AddListener(ButtonClickV1);
            InitializeLocationService();
            InitializeWeatherAPIService();
            InitializeLocationByIPService();
        }
        private void ButtonClickV1()
        {

            locationByIPService.StartService(IPService);
        }

        
        private void InitializeLocationByIPService()
        {
            IPService = new IPService();
            locationByIPService = gameObject.AddComponent<LocationByIPService>();
            locationByIPService.OnComplete += (locationData) => {
                Debug.Log($"locationByIPService {locationData.Latitude}");
                OnLocationReadComplete(locationData);
            };
            locationByIPService.OnFail += () => OnLocationReadFail();
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
            //todo this should be replaced by some dependency injection container -or IPLocationService
            var locationService = new AndroidLocationService();
            weatherLocationService.StartService(locationService);
        }
        private void OnLocationReadComplete(WeatherLocationModel location)
        {
            //todo this should be replaced by some dependency injection container
            var weatherService = new WeatherService();
            weatherService.Latitude = location.Latitude;
            weatherService.Longitude = location.Longitude;
            weatherAPIService.StartService(weatherService);
        }
        private void OnLocationReadFail()
        {
            Toast.ShowToast($"problem reading location");
        }


    }


}

