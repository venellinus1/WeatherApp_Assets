using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.Networking;
using weatherapp.main;

namespace weatherapp.features
{
    public class LocationByIPService : MonoBehaviour, IServiceWithParameters<WeatherLocationModel, IWebRequest>
    {
        public event Action<WeatherLocationModel> OnComplete;
        public event Action OnFail;
        private string locationByIPURL = "https://ipapi.co/json/";
        public void StartService() {
            
        }
        public async void StartService(IWebRequest locationservice) 
        {
            try
            {
                WeatherLocationModel result = await ReadLocation(locationservice);
                OnComplete?.Invoke(result);
            }
            catch
            {
                OnFail?.Invoke();
            }
        }

        public async Task<WeatherLocationModel> ReadLocation(IWebRequest webrequest)
        {
            string url = $"{locationByIPURL}";
            string requestResult = await webrequest.Get(url);
            Debug.Log($"requestResult {requestResult}");
            WeatherLocationModel tmp = WeatherLocationModel.ParseByIP(requestResult);
            Debug.Log($"parsed {tmp.Latitude} {tmp.Longitude}");
            return WeatherLocationModel.ParseByIP(requestResult);
        }
    }

    public class IPService : IWebRequest
    {
        

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
}
