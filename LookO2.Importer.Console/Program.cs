using LookO2.Importer.Console.IoC;
using LookO2.Importer.Core;
using LookO2.Importer.Core.Models.ValueObjects;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LookO2.Importer.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO Add command.dotnet for parsing parameters
            var importingArgs = new BeginImportingArgs(new DateTime(2018, 11, 1), new DateTime(2019, 4, 30));
            var provider = Dependencies.Configure();
            var logger = provider.GetService<ILogger<Program>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            logger.LogInformation($"Starting processing at {DateTime.UtcNow.ToLongTimeString()} (UTC)");
            var importer = provider.GetService<LookO2Importer>();
            await importer.StartAsync(importingArgs);
            logger.LogInformation($"End processing at {DateTime.UtcNow.ToLongTimeString()} (UTC) after {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }
}
