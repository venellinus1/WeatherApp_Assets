using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

using weatherapp.main;

namespace weatherapp.features
{
    public class WeatherAPIService : MonoBehaviour, IServiceWithParameters<string, WeatherLocationModel>
    {
        public event Action<string> OnComplete;
        public event Action OnFail;
        public void StartService() { }
        private const string weatherAPIEndpointUrl = "https://api.open-meteo.com/v1/forecast";

        public async void StartService(WeatherLocationModel locationData)
        {
            try
            {
                string result = await GetWeatherForecast(locationData.Latitude, locationData.Longitude);
                OnComplete?.Invoke(result);
            }
            catch (Exception e)
            {
                OnFail?.Invoke();
            }
        }
        public async Task<string> GetWeatherForecast(float latitude, float longitude)
        {            
            string url = $"{weatherAPIEndpointUrl}?latitude={latitude}&longitude={longitude}&timezone=IST&daily=temperature_2m_max";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {                    
                    OnFail?.Invoke();
                    throw new Exception(webRequest.error);                    
                }
                return webRequest.downloadHandler.text;
            }
        }
    }
}
