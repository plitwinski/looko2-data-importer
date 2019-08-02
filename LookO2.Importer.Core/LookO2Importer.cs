using Akka.Actor;
using Akka.Streams;
using Akka.Streams.Dsl;
using LookO2.Importer.Core.Models;
using LookO2.Importer.Core.Models.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LookO2.Importer.Core
{
    public class LookO2Importer
    {
        private const int ParallelDownloads = 10;

        private readonly IArchivedFileDownloader downloader;

        private readonly IMeterReadingsRepository readingsRepository;

        public LookO2Importer(IArchivedFileDownloader downloader, IMeterReadingsRepository readingsRepository)
        {
            this.downloader = downloader;
            this.readingsRepository = readingsRepository;
        }

        public async Task StartAsync(BeginImportingArgs args)
        {
            // TODO add perf benchmark
            var source = Source.FromEnumerator(() => new ArchivedFileUrlGenerator(args.StartDate, args.EndDate));

            using (var system = ActorSystem.Create("system"))
            using (var materializer = system.Materializer())
            {
                await source.Buffer(ParallelDownloads, OverflowStrategy.Backpressure)
                      .SelectAsync(ParallelDownloads, fileUrl => downloader.DownloadAsync(fileUrl))
                      .SelectAsync(ParallelDownloads, readings => readingsRepository.SaveAsync(readings))
                      .RunWith(Sink.Ignore<IReadOnlyCollection<MeterReading>>(), materializer);
            }
        }
    }
}
