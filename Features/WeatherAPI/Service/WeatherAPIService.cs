using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

using weatherapp.main;

namespace weatherapp.features
{
    public interface IWebRequest
    {
        Task<string> Get(string url);
    }
    public interface IWeatherService : IWeatherLocation, IWebRequest
    {

    }

    public class WeatherService : IWeatherService
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public async Task<string> Get(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new Exception(webRequest.error);
                }
                return webRequest.downloadHandler.text;
            }
        }
    }
    public class WeatherAPIService : MonoBehaviour, IServiceWithParameters<string, IWeatherService>
    {
        public event Action<string> OnComplete;
        public event Action OnFail;
        public void StartService() { }
        private const string weatherAPIEndpointUrl = "https://api.open-meteo.com/v1/forecast";
        
        public async void StartService(IWeatherService weatherService)
        {
            try
            {
                string result = await GetWeatherForecastV1(weatherService);
                OnComplete?.Invoke(result);
            }
            catch (Exception e)
            {
                OnFail?.Invoke();
            }
        }        
        
        public async Task<string> GetWeatherForecastV1(IWeatherService webrequest)
        {
            string url = $"{weatherAPIEndpointUrl}?latitude={webrequest.Latitude}&longitude={webrequest.Longitude}&timezone=IST&daily=temperature_2m_max";
            return await webrequest.Get(url);
        }

    }

}
