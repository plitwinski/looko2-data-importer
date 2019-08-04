using System;
using System.Collections.Generic;
using System.Text;

namespace LookO2.Importer.Console.Tests.Fixtures
{
    internal class ProgramStartupFixture
    {
        private readonly Dictionary<Type, object> overrides;

        public ProgramStartupFixture()
        {
            overrides = new Dictionary<Type, object>();
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
