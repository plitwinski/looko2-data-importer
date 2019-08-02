using LookO2.Importer.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            // TODO read as stream/io pipeline and possibly span??
            var content = await response.Content.ReadAsStringAsync();
            
            var fileLines = content.Split('\n');
            logger.LogInformation($"File downloaded ({fileLines.Length} lines, {response.Content.Headers.ContentLength ?? 0} b)");

            var result = new List<MeterReading>();
            foreach (var line in fileLines)
            {
                var cells = line.Split(',');
                if (cells.Length < 10)
                    continue;

                var isNewVersion = cells.Length > 10;
                var nameIndex = 9;

                var dateTime = DateTimeOffset.Parse($"{cells[0].Trim('"')} {cells[1].Trim('"')}:{cells[2].Trim('"')}:{cells[3].Trim('"')}+01:00");
                var pm1 = double.Parse(cells[6].Trim('"'));
                var pm25 = double.Parse(cells[7].Trim('"'));
                var pm10 = double.Parse(cells[8].Trim('"'));

                double? hcho = null;
                double? temperature = null;
                double? humidity = null;

                if(isNewVersion)
                {
                    nameIndex = 10;
                    hcho = double.Parse(cells[9].Trim('"'));
                    temperature = double.Parse(cells[11].Trim('"'));
                    humidity = double.Parse(cells[12].Trim('"'));
                }

                var device = new MeterDevice(cells[5].Trim('"'), cells[nameIndex].Trim('"'));

                var reading = new MeterReading(dateTime, pm1, pm25, pm10, device, hcho, temperature, humidity);
                result.Add(reading);
            }
            logger.LogInformation($"Parsed into {result.Count} readings");
            return result;
        }
    }
}
