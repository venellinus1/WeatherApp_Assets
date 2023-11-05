using System;
using System.Threading.Tasks;
using UnityEngine;
using venelin.androidutils;
using weatherapp.main;

namespace weatherapp.features
{
    
    public class WeatherLocationService : MonoBehaviour, IService<WeatherLocationModel>
    {
        public event Action<WeatherLocationModel> OnComplete;
        public event Action OnFail;

        public async void StartService()
        {
            try
            {
                WeatherLocationModel result = await ReadLocation();                
                OnComplete?.Invoke(result);
            }
            catch
            {                
                OnFail?.Invoke();
            }
        }
        /*
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

            return new WeatherLocationModel(locationService.lastData.latitude, locationService.lastData.longitude);
        }*/

        private async Task<WeatherLocationModel> ReadLocation()
        {            
            if (!Input.location.isEnabledByUser)
                Toast.ShowToast("Location not enabled on device or app does not have permission to access location");
            // Start the location service
            Input.location.Start(1000f, 1000f);

            // Waits until the location service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {                
                await Task.Delay(TimeSpan.FromSeconds(2));
                Toast.ShowToast("Initializing Location service on phone...");
                maxWait--;
            }
            // If the service didn't initialize in maxWait seconds this cancels location service use
            if (maxWait < 1)
            {                
                Toast.ShowToast($"Timed out trying to initialize Location service");
                OnFail?.Invoke();
                return new WeatherLocationModel(0, 0);
            }
            // If the connection failed this cancels location service use.
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Toast.ShowToast("Unable to determine device location");
                OnFail?.Invoke();
                return new WeatherLocationModel(0, 0);
            }

            return new WeatherLocationModel(Input.location.lastData.latitude, Input.location.lastData.longitude);
        }
    }

    public interface ILocationService
    {
        bool IsEnabledByUser { get; }
        LocationServiceStatus Status { get; }
        void Start(float desiredAccuracyInMeters, float updateDistanceInMeters);
    }

    public class AndroidLocationService : ILocationService
    {
        public bool IsEnabledByUser => Input.location.isEnabledByUser;
        public LocationServiceStatus Status => Input.location.status;

        public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters)
        {
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
        }

        // Add more methods and properties as needed to implement the interface
    }
}