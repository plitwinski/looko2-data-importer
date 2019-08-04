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
            var fixture = new ProgramStartupFixture()
                .SetupCsvResponses(startDate, NoOfDaysToParse, CsvFile.TwoLinesCsvV2)
                .AddMock(() => new ReadingsContextFixture().Create());
            handlerFixture = fixture.HandlerFixture;
           target = fixture.Create();
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
