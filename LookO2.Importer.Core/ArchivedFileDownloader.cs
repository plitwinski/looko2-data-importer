using LookO2.Importer.Core.Extensions;
using LookO2.Importer.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LookO2.Importer.Core
{
    public class ArchivedFileDownloader : IArchivedFileDownloader
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<ArchivedFileDownloader> logger;

        public ArchivedFileDownloader(HttpClient httpClient, ILogger<ArchivedFileDownloader> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<IReadOnlyCollection<MeterReading>> DownloadAsync(string fileUrl)
        {
            logger.LogInformation($"Downloading file from {fileUrl}");
            var response = await httpClient.GetAsync(fileUrl);
            var result = new List<MeterReading>();
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(contentStream))
            {
                while(streamReader.Peek() >= 0)
                {
                    var line = await streamReader.ReadLineAsync();
                    result.Add(GetReading(in line));
                }
            }
            logger.LogInformation($"Parsed into {result.Count} readings");
            return result;
        }

        private MeterReading GetReading(in string line)
        {
            var cells = line.AsSpan().Split(',').GetEnumerator();

            var dateTime = GetDateTime(ref cells);
            cells.MoveNext();
            var deviceId = GetCurrentValue(ref cells);
            var pm1 = double.Parse(GetCurrentValue(ref cells));
            var pm25 = double.Parse(GetCurrentValue(ref cells));
            var pm10 = double.Parse(GetCurrentValue(ref cells));

            double? hcho = null;
            double? temperature = null;
            double? humidity = null;

            var deviceNameOrHcho = GetCurrentValue(ref cells);
            if(double.TryParse(deviceNameOrHcho, out var parsedHcho))
            {
                hcho = parsedHcho;
                deviceNameOrHcho = GetCurrentValue(ref cells);
                temperature = double.Parse(GetCurrentValue(ref cells));
                humidity = double.Parse(GetCurrentValue(ref cells));
            }

            var device = new MeterDevice(deviceId, deviceNameOrHcho);

            return new MeterReading(dateTime, pm1, pm25, pm10, device, hcho, temperature, humidity);
        }

        private DateTimeOffset GetDateTime(ref SplitSpanEnumerator<char> cells)
        {
            var date = GetCurrentValue(ref cells).Split('-').GetEnumerator();

            var year = int.Parse(GetCurrentValue(ref date));
            var month = int.Parse(GetCurrentValue(ref date));
            var day = int.Parse(GetCurrentValue(ref date));

            var hours = int.Parse(GetCurrentValue(ref cells));
            var minutes = int.Parse(GetCurrentValue(ref cells));
            var seconds = int.Parse(GetCurrentValue(ref cells));
            return new DateTimeOffset(year, month, day, hours, minutes, seconds, TimeSpan.FromHours(1));
        }

        private ReadOnlySpan<char> GetCurrentValue(ref SplitSpanEnumerator<char> cells)
        {
            cells.MoveNext();
            return cells.Current.Trim('"');
        }
    }
}
