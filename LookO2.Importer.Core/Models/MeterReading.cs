using System;

namespace LookO2.Importer.Core.Models
{
    public class MeterReading
    {
        public Guid Id { get; }

        public DateTimeOffset DateTime { get; }

        public double PM1 { get; }

        public double PM25 { get; }

        public double PM10 { get; }

        public MeterDevice Device { get; }

        public double? Hcho { get; }

        public double? Temperature { get; }

        public double? Humidity { get; }

        public MeterReading(
            DateTimeOffset dateTime, 
            double pm1, 
            double pm25, 
            double pm10, 
            MeterDevice device, 
            double? hcho, 
            double? temperature, 
            double? humidity)
        {
            Id = Guid.NewGuid();
            DateTime = dateTime;
            PM1 = pm1;
            PM25 = pm25;
            PM10 = pm10;
            Device = device;
            Hcho = hcho;
            Temperature = temperature;
            Humidity = humidity;
        }
    }
}
