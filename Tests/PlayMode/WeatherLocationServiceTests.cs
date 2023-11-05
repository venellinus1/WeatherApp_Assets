using NUnit.Framework;
using UnityEngine;
using weatherapp.features;
using weatherapp.main;
using System;
using System.Diagnostics;
using System.Threading;
public class WeatherLocationServiceTests
{
    private WeatherLocationService _weatherLocationService;
    [SetUp]
    public void SetUp()
    {
        _weatherLocationService = new GameObject().AddComponent<WeatherLocationService>();
    }
    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(_weatherLocationService);
    }

    private class DummyLocationService : ILocationService
    {
        public bool IsEnabledByUser { get; set; }
        public LocationServiceStatus Status { get; set; }
        public float LocationLatitude { get; set; }
        public float LocationLongitude { get; set; }

        public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters)
        {
            //todo recheck tests for start method??
        }
    }    
    
    [Test]
    public void StartService_WhenStarted_DoesNotThrowException()
    {        
        Assert.DoesNotThrow(() => _weatherLocationService.StartService());
    }

    [Test]
    public void StartService_CalledMultipleTimes_InvokesOnCompleteForAllCalls()
    {
        var dummyLocationService = new DummyLocationService
        {
            IsEnabledByUser = true,
            Status = LocationServiceStatus.Running,
            LocationLatitude = 50.0f,
            LocationLongitude = 100.0f
        };

        int onCompleteInvocationCount = 0;
        _weatherLocationService.OnComplete += (locationData) => onCompleteInvocationCount++;

        _weatherLocationService.StartService(dummyLocationService);
        _weatherLocationService.StartService(dummyLocationService);
        _weatherLocationService.StartService(dummyLocationService);
        _weatherLocationService.StartService(dummyLocationService);

        Assert.AreEqual(4, onCompleteInvocationCount);
    }

    [Test]
    public void StartService_CompletesWithCorrectLocation_WhenLocationServiceIsEnabled()
    {
        // Arrange
        var dummyLocationService = new DummyLocationService
        {
            IsEnabledByUser = true,
            Status = LocationServiceStatus.Running,
            LocationLatitude = 50.0f,
            LocationLongitude = 100.0f
        };

        WeatherLocationModel result = null;

        // Subscribe to the OnComplete event
        _weatherLocationService.OnComplete += (location) => { result = location; };

        // Act
        _weatherLocationService.StartService(dummyLocationService);

        var timeout = TimeSpan.FromSeconds(20);
        var stopwatch = Stopwatch.StartNew();
        while (result == null && stopwatch.Elapsed < timeout)
        {
            Thread.Sleep(100);
        }

        // Assert
        Assert.IsNotNull(result, "Timeout: The OnComplete event was not fired within the expected time.");
        Assert.AreEqual(50.0f, result.Latitude);
        Assert.AreEqual(100.0f, result.Longitude);
    }

    

    [Test]
    public void StartService_Fails_WhenLocationServiceIsNotEnabledByUser()
    {
        // Arrange
        var dummyLocationService = new DummyLocationService
        {
            IsEnabledByUser = false,// the service not being enabled by the user
            Status = LocationServiceStatus.Running,
            LocationLatitude = 50.0f,
            LocationLongitude = 100.0f
        };

        bool failed = false;

        //Subscribe to the OnFail event
        _weatherLocationService.OnFail += () => { failed = true; };

        //Act
        _weatherLocationService.StartService(dummyLocationService);

        //Assert
        Assert.IsTrue(failed, "The OnFail event was not fired as expected");
    }

    [Test]
    public void StartService_Fails_WhenLocationServiceStatusIsFailed()
    {
        //Arrange
        var dummyLocationService = new DummyLocationService
        {
            IsEnabledByUser = true,
            Status = LocationServiceStatus.Failed,//should fail if (locationService.Status == LocationServiceStatus.Failed)
            LocationLatitude = 50.0f,
            LocationLongitude = 100.0f
        };

        bool failed = false;

        //Subscribe to the OnFail event
        _weatherLocationService.OnFail += () => { failed = true; };

        //Act
        _weatherLocationService.StartService(dummyLocationService);

        //Assert
        Assert.IsTrue(failed, "The OnFail event was not fired as expected");
    }

}
