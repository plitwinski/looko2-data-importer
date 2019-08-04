using LookO2.Importer.Core;
using LookO2.Importer.Core.Models;
using LookO2.Importer.Core.Tests;
using LookO2.Importer.Tests.Fixtures;
using LookO2.Importer.Tests.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SemanticComparison.Fluent;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    public class ArchivedFileDownloaderTests
    {
        [Test]
        public async Task WhenDownloadAsync_SingleLineCsv_FileDownloadedAndParsed()
        {
            const string filePath = "unit-test/test-file.csv";
            var handler = new HttpMessageHandlerFixture()
                .SetupGetCsv(filePath, Model.MeterReadingLineV2())
                .Create();

            var target = new ArchivedFileDownloader(
                new HttpClient(handler),
                Mock.Of<ILogger<ArchivedFileDownloader>>());

            var result = (await target.DownloadAsync($"http://unit-test.com/{filePath}")).Single();

            result.AsSource()
                .OfLikeness<MeterReading>()
                .Without(p => p.Id)
                .Without(p => p.Device)
                .ShouldEqual(Model.MeterReadingV2);

            result.Device.AsSource()
                .OfLikeness<MeterDevice>()
                .ShouldEqual(Model.MeterReadingV2.Device);
        }

        [TestCase(CsvFile.TwoLinesCsvV1, 2)]
        [TestCase(CsvFile.TwoLinesCsvV2, 2)]
        public async Task WhenDownloadAsync_MultiLineCsv_FileDownloadedAndParsed(string content, int expectedNoOfReadings)
        {
            const string filePath = "unit-test/test-file.csv";
            var handler = new HttpMessageHandlerFixture()
                .SetupGetCsv(filePath, content)
                .Create();

            var target = new ArchivedFileDownloader(
                new HttpClient(handler),
                Mock.Of<ILogger<ArchivedFileDownloader>>());

            var result = await target.DownloadAsync($"http://unit-test.com/{filePath}");

            Assert.AreEqual(expectedNoOfReadings, result.Count);
        }
    }
}