using Microsoft.AspNet.Builder;
using PersonalSite.Middleware;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class XContentTypeOptionsHeaderMiddlewareTests
    {
        [Fact]
        public async Task EmitXFrameOptionsHeaderWithDenyValue()
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXContentTypeOptionsHeader();
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.True(response.Headers.Contains(XContentTypeOptionsHeaderMiddleware.XContentTypeOptionsHeaderName));
            Assert.Equal("nosniff", response.Headers.GetValues(XContentTypeOptionsHeaderMiddleware.XContentTypeOptionsHeaderName).First());
        }
    }
}
