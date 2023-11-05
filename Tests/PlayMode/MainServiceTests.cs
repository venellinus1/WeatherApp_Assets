using NUnit.Framework;
using UnityEngine.TestTools;
using weatherapp.features;
using weatherapp.main;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainerviceTests
{
    [Test]
    public void WeatherAPIModel_Parse_ProvideCorrectString_CheckResultNotNull()
    {
        //Arrange
        var json = "{\"latitude\": 19.125, \"longitude\": 72.875, \"generationtime_ms\": 0.34499168395996094, \"utc_offset_seconds\": 18000, \"timezone\": \"Asia/Calcutta\", \"timezone_abbreviation\": \"IST\", \"elevation\": 6, \"daily_units\": {\"time\": \"iso8601\", \"temperature_2m_max\": \"°C\"}, \"daily\": {\"time\": [\"2022-11-29\", \"2022-11-30\", \"2022-12-01\", \"2022-12-02\", \"2022-12-03\", \"2022-12-04\", \"2022-12-05\"], \"temperature_2m_max\": [32, 34.5, 33.5, 33.5, 33.1, 33.5, 34.1]}}";

        //Act
        var result = WeatherAPIModel.Parse(json);

        //Assert
        Assert.IsNotNull(result);        
    }
    

    [Test]
    public void WeatherAPIModel_WeatherResult_ProvideCorrectDailyData_CheckResult()
    {
        //Arrange
        var geoTemperatureData = new WeatherAPIModel.GeoTemperatureData
        {
            Daily = new WeatherAPIModel.DailyData
            {
                time = new List<string> { "time1", "time2" },
                temperature_2m_max = new List<float> { 1.0f, 2.0f }
            }
        };

        //Act
        var result = WeatherAPIModel.WeatherResult(geoTemperatureData);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("time1", result.DailyTime0);
        Assert.AreEqual(1.0, result.DailyTemperature2mMax0);
    }

}