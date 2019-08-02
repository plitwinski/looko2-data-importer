using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LookO2.Importer.Persistance.Design
{
    public class ReadingsContextFactory : IDesignTimeDbContextFactory<ReadingsContext>
    {
        public ReadingsContext CreateDbContext(string[] args)
        {
            var configuration = GetConfiguration();
            var builder = new DbContextOptionsBuilder<ReadingsContext>()
                                .UseNpgsql(configuration.GetConnectionString("Readings"));
            return new ReadingsContext(builder.Options);
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
