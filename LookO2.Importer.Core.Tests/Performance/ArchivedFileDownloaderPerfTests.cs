using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LookO2.Importer.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LookO2.Importer.Core.Tests.Performance
{
    [Ignore("Don't run perf tests on each test run")]
    [TestFixture(Category = TestCategory.Performance)]
    public class ArchivedFileDownloaderPerfTests
    {
        [MemoryDiagnoser]
        public class ArchivedFileDownloaderWrapper
        {
            private const string filePath = "unit-test/test-file.csv";

            private readonly ArchivedFileDownloader target;

            private static Lazy<string> FileContent = 
                new Lazy<string>(() 
                    => File.ReadAllText(Path.Combine("Performance", "Files", "Archives_2019-01-01.csv")));

            public ArchivedFileDownloaderWrapper()
            {
                var handler = new HttpMessageHandlerFixture()
                    .SetupGetCsv(filePath, FileContent.Value)
                    .Create();

                target = new ArchivedFileDownloader(
                    new HttpClient(handler),
                    Mock.Of<ILogger<ArchivedFileDownloader>>());
            }

            [Benchmark]
            public Task DownloadAsync()
                => target.DownloadAsync($"http://unit-test.com/{filePath}");
        }

        [Test]
        public void WhenDownloadAsync_PerfTest()
        {
            var summary = BenchmarkRunner.Run<ArchivedFileDownloaderWrapper>();
            Assert.Pass($"Results can be found here: {summary.ResultsDirectoryPath}");
        }
    }
}
