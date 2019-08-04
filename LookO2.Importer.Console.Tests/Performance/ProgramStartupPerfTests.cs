using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LookO2.Importer.Console.Tests.Fixtures;
using LookO2.Importer.Tests.Fixtures.Constants;
using NUnit.Framework;
using System;
using System.IO;

namespace LookO2.Importer.Console.Tests.Performance
{
    [Ignore("Don't run perf tests on each test run")]
    [TestFixture(Category = TestCategory.Performance)]
    public class ProgramStartupPerfTests
    {
        [MemoryDiagnoser]
        public class ProgramStartupWrapper
        {
            private readonly ProgramStartup target;

            private DateTime startDate;

            private DateTime endDate;

            private const int NoOfDaysToParse = 20;

            private static Lazy<string> FileContent =
                new Lazy<string>(()
                    => File.ReadAllText(Path.Combine("Performance", "Files", "Archives_2019-01-01.csv")));

            public ProgramStartupWrapper()
            {
                startDate = new DateTime(2017, 1, 1);
                endDate = startDate.AddDays(NoOfDaysToParse - 1);
                target = new ProgramStartupFixture()
                    .SetupCsvResponses(startDate, NoOfDaysToParse, FileContent.Value)
                    .AddMock(() => new ReadingsContextFixture().Create())
                    .Create();
            }

            [Benchmark]
            public void Import()
            {
                target.Start("import",
                "-s", startDate.ToString("yyyy-MM-dd"),
                "-e", endDate.ToString("yyyy-MM-dd"));
            }

        }

        [Test]
        public void WhenImport_PerfTest()
        {
            var summary = BenchmarkRunner.Run<ProgramStartupWrapper>();
            Assert.Pass($"Results can be found here: {summary.ResultsDirectoryPath}");
        }
    }
}
