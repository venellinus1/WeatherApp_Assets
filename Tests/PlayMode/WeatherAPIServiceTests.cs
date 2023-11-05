using NUnit.Framework;
using UnityEngine.TestTools;
using weatherapp.features;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System;
public class WeatherAPIServiceTests
{
    private WeatherAPIService _weatherAPIService;

    [SetUp]
    public void SetUp()
    {
        _weatherAPIService = new GameObject().AddComponent<WeatherAPIService>();
    }
    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(_weatherAPIService);
    }

    public class DummyWeatherService : IWeatherService
    {
        public float Latitude { get; set; } = 19.125f;
        public float Longitude { get; set; } = 72.875f;

        public Task<string> Get(string url)
        {
            return Task.FromResult("{\"latitude\": 19.125, \"longitude\": 72.875, \"generationtime_ms\": 0.34499168395996094, \"utc_offset_seconds\": 18000, \"timezone\": \"Asia/Calcutta\", \"timezone_abbreviation\": \"IST\", \"elevation\": 6, \"daily_units\": {\"time\": \"iso8601\", \"temperature_2m_max\": \"°C\"}, \"daily\": {\"time\": [\"2022-11-29\", \"2022-11-30\", \"2022-12-01\", \"2022-12-02\", \"2022-12-03\", \"2022-12-04\", \"2022-12-05\"], \"temperature_2m_max\": [32, 34.5, 33.5, 33.5, 33.1, 33.5, 34.1]}}");
        }
    }
    public class FaultyWeatherService : IWeatherService
    {
        public float Latitude { get; set; } = 19.125f;
        public float Longitude { get; set; } = 72.875f;

        public Task<string> Get(string url)
        {            
            throw new Exception("Simulated failure");
        }
    }

    [Test]
    public void StartService_WhenCalledWithCorrectData_ShouldInvokeOnComplete()
    {
        // Arrange
        var dummyWeatherService = new DummyWeatherService();
        bool wasOnCompleteInvoked = false;
        _weatherAPIService.OnComplete += result => wasOnCompleteInvoked = true;

        // Act
        _weatherAPIService.StartService(dummyWeatherService);

        // Assert
        Assert.IsTrue(wasOnCompleteInvoked);
    }
    
    [Test]
    public void StartService_ShouldInvokeOnFail_WhenWebRequestThrowsException()
    {
        // Arrange
        var faultyWeatherService = new FaultyWeatherService();
        bool wasOnFailInvoked = false;
        _weatherAPIService.OnFail += () => wasOnFailInvoked = true;

        // Act
        _weatherAPIService.StartService(faultyWeatherService);

        // Assert
        Assert.IsTrue(wasOnFailInvoked);
    }
    [Test]
    public void StartService_WhenStarted_DoesNotThrowException()
    {
        Assert.DoesNotThrow(() => _weatherAPIService.StartService());
    }

    [Test]
    public void StartService_CalledMultipleTimes_InvokesOnCompleteForAllCalls()
    {

        var dummyWeatherService = new DummyWeatherService();
        int onCompleteInvocationCount = 0;
        _weatherAPIService.OnComplete += (locationData) => onCompleteInvocationCount++;

        _weatherAPIService.StartService(dummyWeatherService);
        _weatherAPIService.StartService(dummyWeatherService);
        _weatherAPIService.StartService(dummyWeatherService);
        _weatherAPIService.StartService(dummyWeatherService);

        Assert.AreEqual(4, onCompleteInvocationCount);
    }
}
