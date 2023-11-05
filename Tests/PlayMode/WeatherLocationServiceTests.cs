using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Threading.Tasks;
using weatherapp.features;
using weatherapp.main;
using System.Reflection;
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
        Object.Destroy(_weatherLocationService);
    }

    

    [Test]
    public void StartService_WhenCalled_ShouldInvokeOnComplete()
    {
        bool wasOnCompleteCalled = false;
        _weatherLocationService.OnComplete += (locationData) => wasOnCompleteCalled = true;

        _weatherLocationService.StartService();

        Assert.IsTrue(wasOnCompleteCalled);
    }

    
    [Test]
    public void StartService_WhenStarted_DoesNotThrowException()
    {        
        Assert.DoesNotThrow(() => _weatherLocationService.StartService());
    }

    [Test]
    public void StartService_CalledMultipleTimes_InvokesOnCompleteForAllCalls()
    {
        int onCompleteInvocationCount = 0;
        _weatherLocationService.OnComplete += (locationData) => onCompleteInvocationCount++;

        _weatherLocationService.StartService();
        _weatherLocationService.StartService();
        _weatherLocationService.StartService();
        _weatherLocationService.StartService();

        Assert.AreEqual(4, onCompleteInvocationCount);
    }

    // Define a manual mock implementation of ILocationService for testing
    public class ManualLocationService : ILocationService
    {
        // Properties to control behavior during testing
        public bool IsEnabledByUser { get; set; }
        public LocationServiceStatus Status { get; set; }

        public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters)
        {
            // Simulate the behavior you want for testing
            // For example, set properties to specific values based on the test scenario
        }
    }

    [Test]
    public void TestReadLocation()
    {
        var weatherLocationService = new WeatherLocationService();

        var methodInfo = weatherLocationService.GetType().GetMethod("ReadLocation", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = methodInfo.Invoke(weatherLocationService, new object[] { /* parameter values */ }) as Task<WeatherLocationModel>;

        // Assert and validate the result as needed
    }

}
