using LookO2.Importer.Console.Tests.Fixtures;
using LookO2.Importer.Tests.Fixtures;
using LookO2.Importer.Tests.Responses;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace LookO2.Importer.Console.Tests
{
    public class WhenProgramStartupStarts_AllFilesAreParsed : GivenWhen
    {
        private HttpMessageHandlerFixture handlerFixture;

        private ProgramStartup target;

        private DateTime startDate;

        private DateTime endDate;

        private const int NoOfDaysToParse = 12;

        public override void Given()
        {
            startDate = new DateTime(2017, 1, 1);
            endDate = startDate.AddDays(NoOfDaysToParse - 1);

            handlerFixture = new HttpMessageHandlerFixture();

            for(var i = 0; i < NoOfDaysToParse; i++)
            {
                var date = startDate.AddDays(i);
                handlerFixture
                    .SetupGetCsv($"Archives/{date.Year}/Archives_{date.ToString("yyyy-MM-dd")}.csv", CsvFile.TwoLinesCsvV2);
            }

            target = new ProgramStartupFixture()
                .AddMock(new HttpClient(handlerFixture.Create()))
                .AddMock(() => new ReadingsContextFixture().Create())
                .Create();
        }

        public override void When()
        {
            target.Start("import", 
                "-s", startDate.ToString("yyyy-MM-dd"), 
                "-e", endDate.ToString("yyyy-MM-dd"));
        }

        [Test]
        public void ThenStartFileDownloaded()
        {
            handlerFixture.Mock.Verify(p => p.Send(It.Is<HttpRequestMessage>(x => 
                        x.RequestUri.AbsolutePath
                            .EndsWith($"Archives/{startDate.Year}/Archives_{startDate.ToString("yyyy-MM-dd")}.csv", StringComparison.InvariantCultureIgnoreCase))),
                Times.Once);
        }

        [Test]
        public void ThenEndFileDownloaded()
        { 
            handlerFixture.Mock.Verify(p => p.Send(It.Is<HttpRequestMessage>(x =>
                        x.RequestUri.AbsolutePath
                            .EndsWith($"Archives/{endDate.Year}/Archives_{endDate.ToString("yyyy-MM-dd")}.csv", StringComparison.InvariantCultureIgnoreCase))),
                Times.Once);
        }

        [Test]
        public void ThenExpectedNumberOfFilesDownloaded()
        {
            handlerFixture.Mock.Verify(p => p.Send(It.IsAny<HttpRequestMessage>()), 
                Times.Exactly(NoOfDaysToParse));
        }
    }
}
