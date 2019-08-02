using LookO2.Importer.Persistance.Entitites;
using Microsoft.EntityFrameworkCore;

namespace LookO2.Importer.Persistance
{
    public class ReadingsContext : DbContext
    {
        public ReadingsContext(DbContextOptions<ReadingsContext> options) : base(options) { }

        public DbSet<MeterDeviceEntity> Devices { get; set; }

        public DbSet<MeterReadingEntity> Readings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MeterDeviceEntity>()
                .HasIndex(u => u.DeviceId)
                .IsUnique();
        }
    }
}
