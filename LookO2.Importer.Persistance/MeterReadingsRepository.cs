using LookO2.Importer.Core;
using LookO2.Importer.Core.Models;
using LookO2.Importer.Persistance.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookO2.Importer.Persistance
{
    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly Func<ReadingsContext> contextFactory;

        private readonly ILogger<MeterReadingsRepository> logger;

        public MeterReadingsRepository(Func<ReadingsContext> contextFactory, ILogger<MeterReadingsRepository> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        public async Task<IReadOnlyCollection<MeterReading>> SaveAsync(IReadOnlyCollection<MeterReading> readings)
        {
            var devices = readings
                .GroupBy(p => p.Device.Id)
                .Select(p => p.First().Device)
                .ToList();
            logger.LogInformation($"Detected {devices.Count} devices");

            using (var context = contextFactory())
            {
                await CreateDevicesAsync(devices, context);
                await SaveReadingsAsync(readings, context);
            }
            return readings;
        }

        private async Task SaveReadingsAsync(IReadOnlyCollection<MeterReading> readings, ReadingsContext context)
        {
            var deviceCache = new Dictionary<string, MeterDeviceEntity>();
            foreach (var reading in readings)
            {
                if (!deviceCache.ContainsKey(reading.Device.Id))
                {
                    deviceCache.Add(reading.Device.Id,
                        await context.Devices.SingleAsync(p => p.DeviceId == reading.Device.Id));
                }
                var deviceEntity = deviceCache[reading.Device.Id];
                context.Readings.Add(new MeterReadingEntity()
                {
                    Id = reading.Id,
                    DateTime = reading.DateTime,
                    DeviceId = deviceEntity.Id,
                    PM1 = reading.PM1,
                    PM25 = reading.PM25,
                    PM10 = reading.PM10
                });
            }
            logger.LogInformation($"Adding {readings.Count} readings to database");
            await context.SaveChangesAsync();
            logger.LogInformation($"{readings.Count} readings saved to database");
        }

        private async Task CreateDevicesAsync(IEnumerable<MeterDevice> devices, ReadingsContext context)
        {
            foreach (var device in devices)
            {
                var deviceEntity = await context.Devices.AsNoTracking().FirstOrDefaultAsync(p => p.DeviceId == device.Id);
                if (deviceEntity == null)
                {
                    logger.LogTrace($"Adding new device (device id: {device.Id}, name: {device.Name})");
                    deviceEntity = new MeterDeviceEntity()
                    {
                        DeviceId = device.Id,
                        Name = device.Name
                    };

                    try
                    {
                        context.Devices.Add(deviceEntity);
                        await context.SaveChangesAsync();
                    }
                    catch(DbUpdateException updateEx)
                    {
                        if(!IsUniqueConstrainViolation(updateEx))
                            throw;
                        context.Remove(deviceEntity);
                    }
                    logger.LogTrace($"Device (device id: {device.Id}) added");
                }
            }
        }

        private const string UniqueConstrainViolationErrorCode = "23505";

        private bool IsUniqueConstrainViolation(DbUpdateException updateException)
            => updateException?.InnerException != null
                && updateException.InnerException is PostgresException sqlException
                && sqlException.Code == UniqueConstrainViolationErrorCode;
    }
}
