using LookO2.Importer.Persistance;
using Microsoft.EntityFrameworkCore;
using System;

namespace LookO2.Importer.Console.Tests.Fixtures
{
    internal class ReadingsContextFixture 
    {
        private readonly ReadingsContext context;

        public ReadingsContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReadingsContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new ReadingsContext(optionsBuilder.Options);
        }

        public ReadingsContext Create()
            => context;
    }
}
