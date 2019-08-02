using LookO2.Importer.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookO2.Importer.Core
{
    public interface IMeterReadingsRepository
    {
        Task<IReadOnlyCollection<MeterReading>> SaveAsync(IReadOnlyCollection<MeterReading> readings);
    }
}
