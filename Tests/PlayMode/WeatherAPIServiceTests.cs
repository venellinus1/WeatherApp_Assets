using NUnit.Framework;
using UnityEngine.TestTools;
using weatherapp.features;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
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
        Object.Destroy(_weatherAPIService);
    }

    [UnityTest]
    public IEnumerator StartService_WhenCalled_ShouldInvokeOnComplete()
    {
        bool wasOnCompleteCalled = false;
        _weatherAPIService.OnComplete += (result) => wasOnCompleteCalled = true;
        _weatherAPIService.StartService(new WeatherLocationModel(10, 10));

        yield return new WaitUntil(() => wasOnCompleteCalled);

        Assert.IsTrue(wasOnCompleteCalled);
    }

    
}
