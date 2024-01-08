using System;
using System.Threading.Tasks;
using UnityEngine;
using venelin.androidutils;
using weatherapp.main;

namespace weatherapp.features
{
    
    public class WeatherLocationService : MonoBehaviour, IServiceWithParameters<WeatherLocationModel, ILocationService>
    {
        public event Action<WeatherLocationModel> OnComplete;
        public event Action OnFail;
        public void StartService() { }
        public async void StartService(ILocationService locationService)
        {
            try
            {                
                WeatherLocationModel result = await ReadLocation(locationService);                
                OnComplete?.Invoke(result);
            }
            catch
            {                
                OnFail?.Invoke();
            }
        }
        
        private async Task<WeatherLocationModel> ReadLocation(ILocationService locationService)
        {
            if (!locationService.IsEnabledByUser)
            {
                Toast.ShowToast("Location not enabled on device or app does not have permission to access location");
                OnFail?.Invoke();
                return new WeatherLocationModel(0, 0);
            }

            locationService.Start(1000f, 1000f);

            int maxWait = 20;
            while (locationService.Status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                Toast.ShowToast("Initializing Location service on phone...");
                maxWait--;                
            }

            if (maxWait < 1)
            {
                Toast.ShowToast("Timed out trying to initialize Location service");
                OnFail?.Invoke();
                return new WeatherLocationModel(0, 0);
            }

            if (locationService.Status == LocationServiceStatus.Failed)
            {
                Toast.ShowToast("Unable to determine device location");
                OnFail?.Invoke();
                return new WeatherLocationModel(0, 0);
            }

            return new WeatherLocationModel(locationService.LocationLatitude, locationService.LocationLongitude);
        }       
    }

    public interface ILocationService
    {
        bool IsEnabledByUser { get; }
        LocationServiceStatus Status { get; }
        void Start(float desiredAccuracyInMeters, float updateDistanceInMeters);

        float LocationLatitude { get; }
        float LocationLongitude { get; }
    }

    public class AndroidLocationService : ILocationService
    {
        public bool IsEnabledByUser => Input.location.isEnabledByUser;
        public LocationServiceStatus Status => Input.location.status;

        public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters)
        {
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
        }
        public float LocationLatitude => Input.location.lastData.latitude;
        public float LocationLongitude => Input.location.lastData.longitude;
        //todo recheck if more methods and/or properties needed ...

       
    }
}