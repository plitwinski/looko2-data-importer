using System.Collections.Generic;
using System.Threading.Tasks;
using LookO2.Importer.Core.Models;

namespace LookO2.Importer.Core
{
    public interface IArchivedFileDownloader
    {
        Task<IReadOnlyCollection<MeterReading>> DownloadAsync(string fileUrl);
    }
}