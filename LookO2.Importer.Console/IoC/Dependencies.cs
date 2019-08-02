using LookO2.Importer.Core;
using LookO2.Importer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;

namespace LookO2.Importer.Console.IoC
{
    internal class Dependencies
    {
        public static IServiceProvider Configure()
        {
            var configuration = GetConfiguration();

            var services = new ServiceCollection();
            services.AddDbContext<ReadingsContext>(opts => 
                opts.UseNpgsql(configuration.GetConnectionString("Readings")), 
            ServiceLifetime.Transient);

            services.AddHttpClient();
            services.AddTransient(provider => provider.GetService<IHttpClientFactory>().CreateClient());
            services.AddLogging(c =>
            {
                c.AddConsole();
                c.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning);
            });
            services.AddSingleton(configuration);
            services.AddSingleton<Func<ReadingsContext>>(provider => () => provider.GetService<ReadingsContext>());
            services.AddSingleton<IMeterReadingsRepository, MeterReadingsRepository>();
            services.AddSingleton<IArchivedFileDownloader, ArchivedFileDownloader>();
            services.AddSingleton<LookO2Importer>();
            return services.BuildServiceProvider();
        }

        static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
