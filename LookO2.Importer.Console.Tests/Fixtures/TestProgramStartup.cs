using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LookO2.Importer.Console.Tests.Fixtures
{
    internal class TestProgramStartup : ProgramStartup
    {
        private readonly IDictionary<Type, object> overrides;

        public TestProgramStartup(IDictionary<Type, object> overrides)
        {
            this.overrides = overrides;
        }

        protected override IServiceCollection Configure(IServiceCollection services)
        {
            foreach(var @override in overrides)
            {
                if (@override.Value is Func<object> factory)
                    services.Replace(new ServiceDescriptor(@override.Key, _ => factory(), ServiceLifetime.Transient));
                else
                    services.Replace(new ServiceDescriptor(@override.Key, @override.Value));
            }
            return services;
        }
    }
}
