using CommandDotNet.Attributes;
using LookO2.Importer.Console.Cli.Arguments;
using LookO2.Importer.Core;
using LookO2.Importer.Core.Models.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LookO2.Importer.Console.Cli
{
    [ApplicationMetadata(Description = "Application importing historical data from lookO2")]
    public class LookO2Application
    {
        [InjectProperty]
        public LookO2Importer Importer { get; set; }

        [InjectProperty]
        public ILogger<LookO2Application> Logger { get; set; }

        [ApplicationMetadata(Name = "import", Description = "Import data")]
        public async Task ImportAsync(StartImportArgs args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Logger.LogInformation($"Starting processing at {DateTime.UtcNow.ToLongTimeString()} (UTC)");
            await Importer.StartAsync(new BeginImportingArgs(args.StartDate, args.EndDate));
            Logger.LogInformation($"End processing at {DateTime.UtcNow.ToLongTimeString()} (UTC) after {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }
}
