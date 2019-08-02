using LookO2.Importer.Core.Models;
using System;

namespace LookO2.Importer.Core.Tests
{
    internal static class Model
    {
        public static string MeterReadingLineV2()
            => $@"""{MeterReadingV2.DateTime.ToString("yyyy-MM-dd")}"",""{MeterReadingV2.DateTime.Hour}"",""{MeterReadingV2.DateTime.Minute}"",""{MeterReadingV2.DateTime.Second}"",""{MeterReadingV2.DateTime.Year}"",""{MeterReadingV2.Device.Id}"",""{MeterReadingV2.PM1}"",""{MeterReadingV2.PM25}"",""{MeterReadingV2.PM10}"",""{MeterReadingV2.Hcho}"",""{MeterReadingV2.Device.Name}"",""{MeterReadingV2.Temperature}"",""{MeterReadingV2.Humidity}""";

        public static readonly MeterReading MeterReadingV2
            = new MeterReading(new DateTimeOffset(2019, 8, 2, 1, 2, 3, TimeSpan.FromHours(1)),
                1, 
                2.5, 
                10, 
                new MeterDevice("testId", "testName"), 
                2, 
                10, 
                30);
    }
}
