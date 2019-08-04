using LookO2.Importer.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace LookO2.Importer.Console.Tests.Fixtures
{
    internal class ProgramStartupFixture
    {
        private readonly Dictionary<Type, object> overrides;

        public HttpMessageHandlerFixture HandlerFixture { get; }

        public ProgramStartupFixture()
        {
            overrides = new Dictionary<Type, object>();
            HandlerFixture = new HttpMessageHandlerFixture();
        }

        public ProgramStartupFixture SetupCsvResponses(DateTime startDate, int noOfDays, string responseContent)
        {
            for (var i = 0; i < noOfDays; i++)
            {
                var date = startDate.AddDays(i);
                HandlerFixture
                    .SetupGetCsv($"Archives/{date.Year}/Archives_{date.ToString("yyyy-MM-dd")}.csv", responseContent);
            }

            return AddMock(new HttpClient(HandlerFixture.Create()));
        }

        public ProgramStartupFixture AddMock<T>(T mockedValue)
        {
            overrides.Add(typeof(T), mockedValue);
            return this;
        }

        public ProgramStartupFixture AddMock<T>(Func<T> mockFactory)
        {
            overrides.Add(typeof(T), new Func<object>(() => mockFactory()));
            return this;
        }

        public ProgramStartup Create()
            => new TestProgramStartup(overrides);
    }
}
