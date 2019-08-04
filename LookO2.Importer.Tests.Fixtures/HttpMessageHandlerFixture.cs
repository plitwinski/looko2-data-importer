using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace LookO2.Importer.Tests.Fixtures
{
    public class HttpMessageHandlerFixture : BaseFixture<TestMessageHandler>
    {
        public HttpMessageHandlerFixture() : base(true) { }

        public HttpMessageHandlerFixture SetupGetCsv(string path, string csvContent)
        {
            Mock.Setup(p => p.Send(It.Is<HttpRequestMessage>(
                x => x.Method == HttpMethod.Get
                  && x.RequestUri.AbsolutePath.EndsWith(path, StringComparison.InvariantCultureIgnoreCase))))
                  .Returns(() => new HttpResponseMessage()
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(csvContent, Encoding.UTF8, "text/csv")
                  });

            return this;
        }
    }
}
