using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using LookO2.Importer.Console.Cli;
using LookO2.Importer.Console.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace LookO2.Importer.Console
{
    public class ProgramStartup
    {
        protected virtual IServiceCollection Configure(IServiceCollection services)
            => services;

        public int Start(params string[] args)
        {
            var provider = Configure(Dependencies.Configure())
                .BuildServiceProvider();

            return new AppRunner<LookO2Application>()
                .UseMicrosoftDependencyInjection(provider)
                .Run(args);
        }
    }
}
