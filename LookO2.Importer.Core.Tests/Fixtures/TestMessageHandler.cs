using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LookO2.Importer.Core.Tests.Fixtures
{
    public class TestMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
            => throw new NotSupportedException("This method should be mocked and should NOT be called at all");

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(Send(request));
    }
}
